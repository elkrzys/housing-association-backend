using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace HousingAssociation.DataAccess.Entities
{
    public abstract record Event
    {
        [Key] public int Id { get; set; }
        [MaxLength(255)] public string Title { get; set; }
        [MaxLength(255)] public string Content { get; set; }
        [Required][ForeignKey("Author")] public int AuthorId { get; set; }
        [Required] public DateTime CreatedAt { get; set; }

        [JsonIgnore] public User Author { get; set; }
    }
}