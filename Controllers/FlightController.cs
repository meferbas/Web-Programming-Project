using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirlineSeatReservationSystem.Models;
using AirlineSeatReservationSystem.Entity;
using AirlineSeatReservationSystem.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AirlineSeatReservationSystem.Controllers;

public class FlightController : Controller
{
    private IFlightRepository _repository;
    public FlightController(IFlightRepository repository)
    {
        _repository = repository;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(FlightViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _repository.Flights.FirstOrDefaultAsync(x => x.From == model.From && x.To == model.To);
            if (user == null)
            {
                _repository.getFlight(new Flight
                {
                    From = model.From,
                    To = model.To,
                    Depart = model.Depart,
                    Return = model.Return,
                    Guest = model.Guest

                });

                return RedirectToAction("SignIn");
            }

            


        }
        return View(model);
    }

    [Authorize(Roles = "admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "admin")]

    public IActionResult Create(FlightCreateViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (ModelState.IsValid)
        {
            _repository.getFlight(
                new Flight
                {
                    From = model.From,
                    To = model.To,
                    Depart = Convert.ToString(model.Depart),
                    Return = model.Return,
                    Time = model.Time,
                    Guest = model.Guest,

                }
            );
            return RedirectToAction("ListFlights");
        }
        return View();
    }

   [Authorize]
public async Task<IActionResult> SearchFlights(FlightViewModel model)
{
  
    if (ModelState.IsValid )
    {
        
        var query = _repository.Flights.AsQueryable();

        // Ortak kriterler
        query = query.Where(f => f.From == model.From && f.To == model.To && f.Guest == model.Guest);

        // One Way ise Return tarihini dikkate alma.
        if (model.IsOneWay)
        {
            query = query.Where(f => f.Depart == model.Depart);
        }
        else
        {
            // Return seçeneği varsa ve tarih belirtilmişse
            query = query.Where(f => f.Depart == model.Depart);
            if (!string.IsNullOrEmpty(model.Return))
            {
                query = query.Where(f => f.Return == model.Return);
            }
        }

        var flights = await query.Select(flightEntity => new FlightCreateViewModel
        {
            FlightId = flightEntity.FlightId,
            From = flightEntity.From,
            To = flightEntity.To,
            Depart = flightEntity.Depart,
            Return = flightEntity.Return , // Eğer null ise boş string atayabilirsiniz
            Time = flightEntity.Time,
            Guest = flightEntity.Guest,

        }).ToListAsync();
        

        return View("SearchResults", flights);
    }
    if(!ModelState.IsValid){
        var query = _repository.Flights.AsQueryable();

        // Ortak kriterler
        query = query.Where(f => f.From == model.From && f.To == model.To && f.Guest == model.Guest &&f.Return==null);

        // One Way ise Return tarihini dikkate alma.
        if (model.IsOneWay)
        {
            query = query.Where(f => f.Depart == model.Depart);
        }
        else
        {
            // Return seçeneği varsa ve tarih belirtilmişse
            query = query.Where(f => f.Depart == model.Depart);
            if (!string.IsNullOrEmpty(model.Return))
            {
                query = query.Where(f => f.Return == model.Return);
            }
        }

        var flights = await query.Select(flightEntity => new FlightCreateViewModel
        {
            FlightId = flightEntity.FlightId,
            From = flightEntity.From,
            To = flightEntity.To,
            Depart = flightEntity.Depart,
            Return = flightEntity.Return , // Eğer null ise boş string atayabilirsiniz
            Time = flightEntity.Time,
            Guest = flightEntity.Guest,

        }).ToListAsync();

        return View("SearchResults", flights);

    }

    return View("Index", model);
}






    [Authorize(Roles = "admin")]
    public async Task<IActionResult> ListFlights()
    {
        var flights = await _repository.Flights
            .Select(flightEntity => new FlightCreateViewModel
            {
                FlightId = flightEntity.FlightId,
                From = flightEntity.From,
                To = flightEntity.To,
                Time = flightEntity.Time
                // Burada diğer gerekli alanları da ekleyebilirsiniz
            })
            .ToListAsync();

        return View("SearchResults", flights);
    }



    [Authorize(Roles = "admin")]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var ucus = _repository.Flights.FirstOrDefault(i => i.FlightId == id);
        if (ucus == null)
        {
            return NotFound();
        }


        return View(new FlightCreateViewModel
        {
            FlightId = ucus.FlightId,
            From = ucus.From,
            To = ucus.To,
            Depart = ucus.Depart,
            Return = ucus.Return,
            Time = ucus.Time,
            Guest = ucus.Guest


        });
    }


    [Authorize(Roles = "admin")]
    [HttpPost]
    public IActionResult Edit(FlightCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var entityToUpdate = new Flight
            {
                FlightId = model.FlightId,
                From = model.From,
                To = model.To,
                Depart = model.Depart,
                Return = model.Return,
                Time = model.Time,
                Guest = model.Guest

            };
            _repository.editFlight(entityToUpdate);
            return RedirectToAction("ListFlights");
        }
        return View();
    }



    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Delete(int? flightId)
    {
        if (flightId == null)
        {
            return NotFound(); // Uçuş ID'si sağlanmadıysa, 404 hatası döndür
        }

        var flight = await _repository.Flights.FirstOrDefaultAsync(f => f.FlightId == flightId.Value);
        if (flight == null)
        {
            return NotFound(); // Uçuş bulunamadıysa, 404 hatası döndür
        }

        await _repository.DeleteFlight(flight); // DeleteFlight metodunu asenkron hale getirin
        return RedirectToAction("ListFlights"); // Uçuş silindikten sonra, uçuş listesine yönlendir
    }



}