using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.DataAccess.Entities
{
    public record Building
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(255)] public string Number { get; set; }
        [Required] public Address Address { get; set; }
        [Required] public BuildingType Type { get; set; }
        
        public List<Local> Locals { get; set; }
        public List<Announcement> Announcements { get; set; }
    }
}