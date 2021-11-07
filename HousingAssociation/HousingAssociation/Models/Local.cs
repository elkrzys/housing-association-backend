using System.Collections.Generic;

namespace HousingAssociation.Models
{
    public class Local
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public float Area { get; set; }
        public int BuildingId { get; set; }
        public bool IsFullyOwned { get; set; }
        public List<User> Residents { get; set; }
        public List<User> RealOwners { get; set; }
    }
}