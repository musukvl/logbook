using System;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

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
        
        public Point Location { get; set; }
        public Route Route { get; set; }
        
        public string Description { get; set;}
        
        public CustomFields CustomFields { get; set; }
    }

    public class CustomFields
    {
        
    }
}
