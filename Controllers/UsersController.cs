using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AirlineSeatReservationSystem.Data;
using AirlineSeatReservationSystem.Data.Concrete.Efcore;
using AirlineSeatReservationSystem.Data.Abstract;
using AirlineSeatReservationSystem.Entity;

using AirlineSeatReservationSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace AirlineSeatReservationSystem.Controllers
{
    public class UsersController : Controller
    {

        private readonly IBookingRepository _bookingRepository;

        private readonly IUserRepository _userRepository;
        public UsersController(IBookingRepository bookingRepository, IUserRepository usersRepository)
        {
            _bookingRepository = bookingRepository;

            _userRepository = usersRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userRepository.Users.ToListAsync());
        }
        // [Authorize]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        // [Authorize]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.CreateUser(new User
                    {

                        UserName = model.UserName,
                        Phone = model.Phone,
                        Email = model.Email,
                        Password = model.Password,

                    });

                    return RedirectToAction("SignIn");
                }
                else
                {
                    ModelState.AddModelError("", "Email and password already exist");
                }

            }
            return View(model);

        }
        public IActionResult SignIn()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Flight");
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (isUser != null)
                {
                    var useClaims = new List<Claim>();
                    useClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserNo.ToString()));
                    useClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                    useClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.Email ?? ""));
                    if (isUser.Email == "g211210013@sakarya.edu.tr" && isUser.Password == "sau")
                    {
                        useClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    var claimsIdentity = new ClaimsIdentity(useClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );
                    return RedirectToAction("Index", "Flight");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect");
                }
            }

            return View(model);
        }

        // POST: /Booking/BookSeat
        
        public IActionResult BookSeat (int flightId, int seatId)
        {
            // Oturumdan kullanıcı ID'sini al (örneğin User.Identity.GetUserId() ile)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userNo = int.Parse(userIdClaim.Value); // Claim'den alınan değeri int'e çevir

                // Yeni bir Booking nesnesi oluştur
                var booking = new Booking
                {
                    UserNo = userNo,
                    FlightId = flightId,
                    SeatId = seatId,
                    BookingDate = DateTime.UtcNow // Mevcut UTC zamanı kullan
                };

                // Booking nesnesini veritabanına ekle ve değişiklikleri kaydet
                _bookingRepository.Add(booking);
                _bookingRepository.SaveChanges();

                // Başarılı bir yanıt döndür
                return Json(new { success = true });
            }
            else
            {
                // Kullanıcı ID'si bulunamadıysa bir hata mesajı döndür
                return Json(new { success = false, message = "User not authenticated." });
            }
        }


    }
}