using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace HousingAssociation.DataAccess.Entities
{
    public abstract record Event
    {
        [Key] public int Id { get; set; }
        [Required][MaxLength(255)] public string Title { get; set; }
        [Required][MaxLength(1000)] public string Content { get; set; }
        [Required][ForeignKey("Author")] public int AuthorId { get; set; }
        [Required] public DateTimeOffset Created { get; set; }
        public User Author { get; set; }
    }
}