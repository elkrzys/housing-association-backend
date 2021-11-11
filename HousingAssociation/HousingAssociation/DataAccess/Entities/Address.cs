using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.DataAccess.Entities
{
    public record Address
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(255)] public string City { get; set; }
        [MaxLength(255)] public string District { get; set; }
        [Required] [MaxLength(255)] public string Street { get; set; }
    }
}