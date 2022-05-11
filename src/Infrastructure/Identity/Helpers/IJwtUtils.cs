using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Identity.Helpers
{
    public interface IJwtUtils
    {
        public string GenerateToken(string userId, string name, string[] permissions);
        public JwtSecurityToken ValidateToken(string token);
    }
}
