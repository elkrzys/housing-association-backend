using System.ComponentModel.DataAnnotations;
using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Controllers.Requests
{
    public class RegisterRequest
    {
        [Required] [MaxLength(255)] public string FirstName { get; set; }
        [Required] [MaxLength(255)] public string LastName { get; set; }
        [Required] [MaxLength(10)] public string PhoneNumber { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
        //[Required] public Role Role { get; set; }
        [Required] [MaxLength(255)] [DataType(DataType.Password)] public string Password { get; set; }
        //[Required] [MaxLength(255)] [DataType(DataType.Password)][Compare("Password")] public string ConfirmPassword { get; set; }
    }
}