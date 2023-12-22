using AirlineSeatReservationSystem.Entity;
namespace  AirlineSeatReservationSystem.Data.Abstract
{
    public interface IBookingRepository
    {
        IQueryable<Booking> Bookings {get;}

            IQueryable<Booking> GetBookingsByUserId(int userId);
            void Add(Booking booking);
    void SaveChanges();


    }
}