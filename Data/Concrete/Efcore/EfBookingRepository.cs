using AirlineSeatReservationSystem.Entity;
using AirlineSeatReservationSystem.Data.Abstract;
using AirlineSeatReservationSystem.Data.Concrete.Efcore;
using Microsoft.EntityFrameworkCore;

namespace AirlineSeatReservationSystem.Data.Concrete
{
    public class EfBookingRepository : IBookingRepository
    {

        private readonly DataContext context;

        public EfBookingRepository(DataContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Booking> Bookings => context.Bookings;

        public IQueryable<Booking> GetBookingsByUserId(int userId)
        {
            return context.Bookings
                           .Where(b => b.UserNo == userId)
                           .Include(b => b.Flight)
                           .Include(b => b.Seat)
                           .AsQueryable();
        }
        public void Add(Booking booking)
        {
            context.Bookings.Add(booking);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

    }
}