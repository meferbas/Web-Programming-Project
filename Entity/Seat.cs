using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

namespace AirlineSeatReservationSystem.Entity;

public class Seat
{

    [Key]
    public int SeatId { get; set; }

    [ForeignKey("Flight")]
    public int FlightId { get; set; } // Hangi uçuşa ait olduğunu gösterir

    public bool IsOccupied { get; set; } // Koltuğun dolu olup olmadığını belirtir

    public string? SeatNumber { get; set; }

    // İlişkili uçuş entity'si için bir navigation property
    public virtual Flight Flight { get; set; }
}
