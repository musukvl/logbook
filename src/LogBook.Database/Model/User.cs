using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogBook.Database.Model
{
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public ICollection<Route> Routes { get; set; } 
    }
}
