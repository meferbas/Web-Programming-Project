using AirlineSeatReservationSystem.Entity;
using AirlineSeatReservationSystem.Data.Abstract;
using AirlineSeatReservationSystem.Data.Concrete.Efcore;

namespace AirlineSeatReservationSystem.Data.Concrete
{
    public class EfFlightRepository : IFlightRepository
    {

        private DataContext _context;
        public EfFlightRepository(DataContext context)
        {
            _context = context;
        }
        public IQueryable<Flight> Flights => _context.Flights;

        public async Task DeleteFlight(Flight flight)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync(); // Asenkron kaydetme
        }


        public void editFlight(Flight Flight)
        {
            var entity = _context.Flights.FirstOrDefault(i => i.FlightId == Flight.FlightId);
            if (entity != null)
            {
                entity.From = Flight.From;
                entity.To = Flight.To;
                entity.Depart = Flight.Depart;
                entity.Return = Flight.Return;
                entity.Time = Flight.Time;
                entity.Guest = Flight.Guest;
                _context.SaveChanges();
            }
        }

        public void getFlight(Flight Flight)
        {
            _context.Flights.Add(Flight);
            _context.SaveChanges();
        }

    }
}