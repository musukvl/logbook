using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogBook.Database.Model
{
    public class Route
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        public ICollection<WayPoint> WayPoints { get; set; }
    }
}
