using System.ComponentModel.DataAnnotations;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Models.DTOs
{
    public record BuildingDto
    {
        public int? Id { get; set; }
        [Required] [MaxLength(255)] public string Number { get; set; }
        [Required] public Address Address { get; set; }
        [Required] public BuildingType Type { get; set; }
        
        public int? NumberOfLocals { get; set; }

    }
}