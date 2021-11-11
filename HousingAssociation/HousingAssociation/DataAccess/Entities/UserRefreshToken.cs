using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingAssociation.DataAccess.Entities
{
    public class UserRefreshToken
    {
        [Key][ForeignKey("User")] public int UserId { get; set; }
        [Required] public string RefreshToken { get; set; }
        
        public User User { get; set; }
    }
}