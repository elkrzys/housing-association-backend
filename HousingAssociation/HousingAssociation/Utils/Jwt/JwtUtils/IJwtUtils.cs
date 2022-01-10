using HousingAssociation.DataAccess.Entities;

namespace HousingAssociation.Utils.Jwt.JwtUtils
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public RefreshToken GenerateRefreshToken();
    }
}