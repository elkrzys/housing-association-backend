using System;
using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.DataAccess.Models
{
    public abstract class Event
    {
        public int Id { get; set; }
        public EventType Type { get; set; }
        
        [MaxLength(255)]
        public string Title { get; set; }
        
        [MaxLength(255)]
        public string Content { get; set; }
        
        [MaxLength(255)]
        public User Author { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}