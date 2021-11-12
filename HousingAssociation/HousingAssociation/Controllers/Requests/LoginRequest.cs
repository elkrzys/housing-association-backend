using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.Controllers.Requests
{
    public class LoginRequest
    {
        [Required] [EmailAddress] public string Email { get; set; }
        [Required] [MaxLength(255)] public string Password { get; set; }
    }
}