using AirlineSeatReservationSystem.Entity;
using AirlineSeatReservationSystem.Data.Abstract;
using AirlineSeatReservationSystem.Data.Concrete.Efcore;
using Microsoft.EntityFrameworkCore;

namespace AirlineSeatReservationSystem.Data.Concrete
{
    public class EfSeatRepository : ISeatRepository
    {

        private readonly DataContext _context;

    public EfSeatRepository(DataContext context)
    {
        _context = context;
    }

    public IQueryable<Seat> Seats => _context.Seats;

    public async Task<IEnumerable<Seat>> GetSeatsByFlight(int flightId)
    {
        return await _context.Seats.Where(s => s.FlightId == flightId && !s.IsOccupied).ToListAsync();
    }

    public async Task ReserveSeat(int seatId)
    {
        var seat = await _context.Seats.FindAsync(seatId);
        if (seat != null && !seat.IsOccupied)
        {
            seat.IsOccupied = true;
            _context.Update(seat);
            await _context.SaveChangesAsync();
        }
    }
    }
}