using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace AirlineSeatReservationSystem.Models
{
    public class FlightCreateViewModel
    {
        [Key]
        public int FlightId { get; set; }
        [Required]
        [Display(Name = "From")]
        public string? From { get; set; }
        [Required]
        [Display(Name = "To")]
        public string? To { get; set; }
        [Required]
        [Display(Name = "Depart")]
        public string? Depart { get; set; }
        
        [Display(Name = "Return")]
        public string? Return { get; set; }

        [Display(Name = "Time")]
        public string? Time { get; set; }
        [Required]
        [Display(Name = "Guest")]
        public string? Guest { get; set; }
    }
}