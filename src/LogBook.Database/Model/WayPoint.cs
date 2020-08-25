using System;
using System.ComponentModel.DataAnnotations;

namespace LogBook.Database.Model
{
    public class WayPoint
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid RouteId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        public Route Route { get; set; }
    }
}
