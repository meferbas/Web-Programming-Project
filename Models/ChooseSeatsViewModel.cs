using System.Collections.Generic;

namespace AirlineSeatReservationSystem.Models
{
    public class ChooseSeatsViewModel
    {
        public int FlightId { get; set; }
        public List<SeatModel> Seats { get; set; }
        public int SelectedSeat { get; set; } //
    }
}
