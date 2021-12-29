using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.Models.DTOs
{
    public class AddressDto
    {
        public int Id { get; set; }
        [MaxLength(255)] public string City { get; set; }
        [MaxLength(255)] public string District { get; set; }
        [MaxLength(255)] public string Street { get; set; }
    }
}