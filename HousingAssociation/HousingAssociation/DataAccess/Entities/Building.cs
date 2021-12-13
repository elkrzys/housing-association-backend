using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace HousingAssociation.DataAccess.Entities
{
    public record Building
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(255)] public string Number { get; set; }
       
        [ForeignKey("Address")] [Required] public int AddressId { get; set; }
        [Required] public Address Address { get; set; }
        [Required] public BuildingType Type { get; set; }
        
        [JsonIgnore] public List<Local> Locals { get; set; }
        [JsonIgnore] public List<Announcement> Announcements { get; set; }
    }
}