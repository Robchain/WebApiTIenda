using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiTIenda.Domian.Entities;

namespace WebApiTIenda.Infrastucture.Security
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetJWT(User user)
        {

            var userClain = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClain,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credential
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
