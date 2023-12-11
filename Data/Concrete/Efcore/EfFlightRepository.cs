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
        public void getFlight(Flight Flight)
        {
            _context.Flights.Add(Flight);
            _context.SaveChanges();
        }
        
    }
}