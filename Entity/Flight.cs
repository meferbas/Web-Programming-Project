using System.ComponentModel.DataAnnotations;

namespace AirlineSeatReservationSystem.Entity;

        public class Flight
        {
            [Key]
            public int FlightId { get; set; }
            
            public string? From { get; set; }
            public string? To  { get; set; }
            public string? Depart  { get; set; }
            public string? Return  { get; set; }
            public string? Time  { get; set; }
            public string? Guest  { get; set; }
           
        }
