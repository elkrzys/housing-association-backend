using System;

namespace HousingAssociation.Models
{
    public abstract class Event
    {
        public int Id { get; set; }
        public EventType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}