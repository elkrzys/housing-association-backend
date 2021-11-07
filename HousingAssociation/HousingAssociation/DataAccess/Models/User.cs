using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace HousingAssociation.DataAccess.Models
{
    public record User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        
        [Required]
        public Role Role { get; set; }
        public List<Local> OwnedLocals { get; set; }
        public List<Local> ResidedLocals { get; set; }

        [Required] 
        public bool IsEnabled { get; set; } = false;
        public UserCredentials UserCredentials { get; set; }
    }
}