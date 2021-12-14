using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace HousingAssociation.DataAccess.Entities
{
    //[Owned]
    public record RefreshToken
    {
        [Key] [JsonIgnore] public int Id { get; set; }
        [ForeignKey("User")] public int UserId { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expires { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Revoked { get; set; }
        public string ReplacedByToken { get; set; }
        public string ReasonRevoked { get; set; }
        public bool IsExpired => DateTimeOffset.UtcNow >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsRevoked && !IsExpired;
        
        public User User { get; set; }
    }
}