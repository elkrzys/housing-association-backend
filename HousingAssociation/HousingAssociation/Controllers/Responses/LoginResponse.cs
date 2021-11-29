using System;
using HousingAssociation.DataAccess.Entities;
using Newtonsoft.Json;

namespace HousingAssociation.Controllers.Responses
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string Role { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public LoginResponse(User user, string accessToken, string refreshToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Role = Enum.GetName(user.Role);
        }
    }
}