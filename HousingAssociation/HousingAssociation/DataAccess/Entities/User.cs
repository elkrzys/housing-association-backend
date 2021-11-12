using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace HousingAssociation.DataAccess.Entities
{
    public record User
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(255)] public string FirstName { get; set; }
        [Required] [MaxLength(255)] public string LastName { get; set; }
        [Required] [MaxLength(10)] public string PhoneNumber { get; set; }
        [Required] [MaxLength(255)] public string Email { get; set; }
        [Required] public Role Role { get; set; }
        [Required] public bool IsEnabled { get; set; }
        public List<Local> OwnedLocals { get; set; }
        public List<Local> ResidedLocals { get; set; }
        public UserCredentials UserCredentials { get; set; }
        public List<Document> Documents { get; set; }
        public Document Document { get; set; }
        public Issue Issue { get; set; }
    }
}