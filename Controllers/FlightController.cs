using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirlineSeatReservationSystem.Models;
using AirlineSeatReservationSystem.Entity;
using AirlineSeatReservationSystem.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AirlineSeatReservationSystem.Controllers;

public class FlightController : Controller
{
    private IFlightRepository _repository;
    public FlightController(IFlightRepository repository)
    {
        _repository=repository;
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
                var user = await _repository.Flights.FirstOrDefaultAsync(x=> x.From==model.From && x.To==model.To);
                if (user == null)
                {
                    _repository.getFlight(new Flight
                    {
                        From=model.From,
                        To=model.To,
                        Depart=model.Depart,
                        Return=model.Return,
                        Guest=model.Guest

                    });

                    return RedirectToAction("SignIn");
                }
                

            }
            return View(model);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(FlightCreateViewModel model)
        {   
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(ModelState.IsValid)
            {
                _repository.getFlight(
                    new Flight{
                        From=model.From,
                        To=model.To,
                        Depart=model.Depart,
                        Return=model.Return,
                        Time=model.Time,
                        Guest=model.Guest,
            
                    }
                );
                return RedirectToAction("Flight");
            }
            return View();
        }

        
        public async Task<IActionResult> List()
        {
            var flightId=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role=User.FindFirstValue(ClaimTypes.Role);
            var flight=_repository.Flights;
            
            if(string.IsNullOrEmpty(role))
            {
                flight=flight.Where(i=>i.FLightId==flightId);
            }

            return View(await flight.ToListAsync());
        }
}