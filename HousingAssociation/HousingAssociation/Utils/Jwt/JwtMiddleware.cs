using System.Linq;
using System.Threading.Tasks;
using HousingAssociation.Services;
using HousingAssociation.Utils.Jwt.JwtUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HousingAssociation.Utils.Jwt
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        // private readonly JwtConfig _jwtConfig;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtConfig> jwtConfig)
        {
            _next = next;
            // _jwtConfig = jwtConfig.Value;
        }

        public async Task Invoke(HttpContext context, UsersService usersService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = usersService.FindUserById(userId.Value);
            }

            await _next(context);
        }
    }
}