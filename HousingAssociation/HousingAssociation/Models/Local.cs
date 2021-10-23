using System.Collections.Generic;

namespace HousingAssociation.Models
{
    public class Local
    {
        public int Number { get; set; }
        public float Area { get; set; }
        public List<User> Residents { get; set; }
        public List<User> RealOwners { get; set; }
    }
}