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
using AirlineSeatReservationSystem.Services;
using Microsoft.AspNetCore.Localization;


namespace AirlineSeatReservationSystem.Controllers
{
    public class UsersController : Controller
    {

        private readonly IBookingRepository _bookingRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ILogger<UsersController> _logger;

        private readonly LanguageService _localization;
        private readonly IUserRepository _userRepository;
        private readonly IFlightRepository _flightRepository;
        public UsersController(IBookingRepository bookingRepository, IUserRepository usersRepository, IFlightRepository flightRepository, ISeatRepository seatRepository, ILogger<UsersController> logger, LanguageService localization)
        {
            _bookingRepository = bookingRepository;

            _userRepository = usersRepository;
            _flightRepository = flightRepository;

            _seatRepository = seatRepository;
            _logger = logger;
            _localization = localization;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserName = _localization.Getkey("UserName").Value;
            ViewBag.Phone = _localization.Getkey("Phone").Value;
            ViewBag.Password = _localization.Getkey("Password").Value;
            ViewBag.Repeat = _localization.Getkey("Repeat").Value;
            ViewBag.SignIn = _localization.Getkey("Login").Value;
            ViewBag.SignIn = _localization.Getkey("Sign Up").Value;




            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            return View(await _userRepository.Users.ToListAsync());
        }
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)

            });
            return Redirect(Request.Headers["Referer"].ToString());
        }

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
                    if (isUser.Email == "g211210013@sakarya.edu.tr" && isUser.Password == "sau" || isUser.Email == "g201210093@sakarya.edu.tr" && isUser.Password == "sau")
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


    }
}