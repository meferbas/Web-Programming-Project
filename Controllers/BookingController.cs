using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirlineSeatReservationSystem.Models;
using AirlineSeatReservationSystem.Entity;
using AirlineSeatReservationSystem.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AirlineSeatReservationSystem.Services;
using Microsoft.AspNetCore.Localization;

namespace AirlineSeatReservationSystem.Controllers;

public class BookingController : Controller
{
    private IBookingRepository _repository;
    private readonly ILogger<BookingController> _logger;

    private readonly LanguageService _localization;


    public BookingController(IBookingRepository repository, ILogger<BookingController> logger, LanguageService localization)
    {
        _repository = repository;
        _logger = logger;
        _localization = localization;
    }
    public IActionResult Index()
    {
        ViewBag.From = _localization.Getkey("From").Value;
        ViewBag.To = _localization.Getkey("To").Value;
        ViewBag.DepartureTime = _localization.Getkey("Departure Time").Value;
        ViewBag.DepartureDate = _localization.Getkey("Departure Date").Value;
        ViewBag.ReturnDate = _localization.Getkey("Return Date").Value;
        ViewBag.SeatNumber = _localization.Getkey("Seat Number").Value;
        ViewBag.SeatNumber = _localization.Getkey("Seat").Value;



        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
        return View();
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

    public IActionResult MyBookings()
    {
        // Oturumda giriş yapmış kullanıcının ID'sini al
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            // Kullanıcı giriş yapmamışsa veya ID claim'i bulunamıyorsa, hata döndür veya uygun bir sayfaya yönlendir
            return RedirectToAction("Index", "Flight"); // Örnek bir yönlendirme
        }

        var userId = int.Parse(userIdClaim.Value); // Claim'den alınan değeri int'e çevir

        // Kullanıcının rezervasyonlarını repository'den al ve ilişkili Flight ve Seat bilgilerini yükle
        var bookingsList = _repository.GetBookingsByUserId(userId)
            .Include(b => b.Flight)
            .Include(b => b.Seat)
            .ToList();

        // ViewModel'i oluştur ve rezervasyon listesini ata
        var viewModel = new MyBookingsViewModel
        {
            Bookings = bookingsList
        };

        // ViewModel'i görünüme gönder
        return View(viewModel);
    }



}