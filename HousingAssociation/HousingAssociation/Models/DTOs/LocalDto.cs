using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Models.DTOs
{
    public class LocalDto
    {
        public int? Id { get; set; }
        public string Number { get; set; }
        public float? Area { get; set; }
        public int BuildingId { get; set; }
        public string BuildingNumber { get; set; }
        public bool? IsFullyOwned { get; set; }
        public int? NumberOfResidents { get; set; }
        public Address Address { get; set; }
    }
}