using AirlineSeatReservationSystem.Entity;
namespace AirlineSeatReservationSystem.Data.Abstract
{
    public interface ISeatRepository
    {
        IQueryable<Seat> Seats { get; }
        Task<IEnumerable<Seat>> GetSeatsByFlight(int flightId);
        Task ReserveSeat(int seatId);


    }
}