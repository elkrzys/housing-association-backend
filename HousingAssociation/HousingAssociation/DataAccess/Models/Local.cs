using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Models
{
    public record Local
    {
        public int Id { get; set; }
        
        public int Number { get; set; }
        public float Area { get; set; }
        public int BuildingId { get; set; }
        public bool IsFullyOwned { get; set; }
        public List<User> Residents { get; set; }
        public List<User> Owners { get; set; }
    }
}