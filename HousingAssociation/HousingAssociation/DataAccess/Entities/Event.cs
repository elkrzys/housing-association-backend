using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Entities
{
    public abstract class Event
    {
        [Key] public int Id { get; set; }
        [Required] public EventType Type { get; set; }
        [MaxLength(255)] public string Title { get; set; }
        [MaxLength(255)] public string Content { get; set; }
        [Required][ForeignKey(nameof(User))] public int AuthorId { get; set; }
        [Required] public DateTime CreatedAt { get; set; }

        public User Author { get; set; }
    }
}