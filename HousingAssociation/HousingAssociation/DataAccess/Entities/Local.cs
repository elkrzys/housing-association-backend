using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HousingAssociation.DataAccess.Entities
{
    public record Local
    {
        [Key] public int Id { get; set; }
        public int? Number { get; set; }
        public float? Area { get; set; }
        public int BuildingId { get; set; }
        public bool IsFullyOwned { get; set; }
        [JsonIgnore] public List<User> Residents { get; set; }
        [JsonIgnore] public List<User> Owners { get; set; }
    }
}