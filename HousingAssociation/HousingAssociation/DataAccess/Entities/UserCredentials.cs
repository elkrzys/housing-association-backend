using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Entities
{
    public record UserCredentials
    {
        [Key][ForeignKey("User")] public int UserId { get; set; }
        [Required][MaxLength(255)] public string PasswordHash { get; set; }
        public User User { get; set; }
    }
}