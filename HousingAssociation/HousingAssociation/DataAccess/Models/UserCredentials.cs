using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Models
{
    public record UserCredentials
    {
        [Key]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }
    }
}