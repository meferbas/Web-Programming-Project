using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace AirlineSeatReservationSystem.Models
{
    public class SeatModel
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; } // Artık string türünde
        public bool IsOccupied { get; set; }
    }

}