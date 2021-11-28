using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.Controllers.Requests
{
    public class ResetPasswordRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string Password { get; set; }
    }
}