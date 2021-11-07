using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.DataAccess.Models
{
    public record Address
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string City { get; set; }
        
        [MaxLength(255)]
        public string District { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Street { get; set; }
    }
}