using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.DataAccess.Models
{
    public record Building
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Number { get; set; }
        
        [Required]
        public Address Address { get; set; }
        
        public List<Local> Locals { get; set; }
        
        [Required]
        public BuildingType Type { get; set; }
    }
}