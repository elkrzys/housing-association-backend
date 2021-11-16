using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Models.DTOs
{
    public record UserDto
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(255)] public string FirstName { get; set; }
        [Required] [MaxLength(255)] public string LastName { get; set; }
        [Required] [MaxLength(10)] public string PhoneNumber { get; set; }
        [Required] [MaxLength(255)] public string Email { get; set; }
        [Required] public Role Role { get; set; }
        [Required] public bool IsEnabled { get; set; }
    }
}