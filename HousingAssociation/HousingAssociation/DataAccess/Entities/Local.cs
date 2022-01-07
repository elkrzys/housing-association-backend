using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HousingAssociation.DataAccess.Entities
{
    public record Local
    {
        [Key] public int Id { get; set; }
        public string Number { get; set; }
        public float? Area { get; set; }
        [ForeignKey("Building")] public int BuildingId { get; set; }
        public bool IsFullyOwned { get; set; }
        public List<User> Residents { get; set; }
        public List<Issue> Issues { get; set; }
        public Building Building { get; set; }
        
    }
}