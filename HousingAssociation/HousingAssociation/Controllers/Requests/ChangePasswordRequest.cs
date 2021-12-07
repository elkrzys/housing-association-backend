using System.ComponentModel.DataAnnotations;

namespace HousingAssociation.Controllers.Requests
{
    public class ChangePasswordRequest
    {
        [Required] public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }
    }
}