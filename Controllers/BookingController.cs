using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirlineSeatReservationSystem.Models;
using AirlineSeatReservationSystem.Entity;
using AirlineSeatReservationSystem.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AirlineSeatReservationSystem.Controllers;

public class BookingController : Controller
{
    private IBookingRepository _repository;
    public BookingController(IBookingRepository repository)
    {
        _repository = repository;
    }
    public IActionResult Index()
    {
        return View();
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