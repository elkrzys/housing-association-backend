using System.Collections.Generic;

namespace HousingAssociation.Models
{
    public class Building
    {
        public string Number { get; set; }
        public Address Address { get; set; }
        public List<Local> Locals { get; set; }
        public BuildingType Type { get; set; }
    }
}