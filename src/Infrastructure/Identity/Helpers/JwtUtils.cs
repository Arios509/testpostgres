using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Identity.Helpers
{
    public class JwtUtils : IJwtUtils
    {
        private readonly JwtOptions _jwtOptions;

        public JwtUtils(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(string userId, string name, string[] permissions)
        {
            var symmetricKey = new HMACSHA256(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var claims = new[] {
                new Claim("userId", userId),
                new Claim("name", name),
                new Claim("permissions", JsonSerializer.Serialize(permissions))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _jwtOptions.ValidAudience,
                Expires = DateTime.UtcNow.AddDays(_jwtOptions.MaxAge),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey.Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new HMACSHA256(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key.Key),
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidAudience = _jwtOptions.ValidAudience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken;
            }
            catch
            {
                return null;
            }
        }
    }
}
