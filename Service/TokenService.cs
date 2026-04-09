using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentaCarAPI.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Client> _userManager;
        private readonly SymmetricSecurityKey _Key;
        public TokenService(IConfiguration configuration, UserManager<Client> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));
        }
        public async Task<string> CreateToken(Client client)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, client.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, client.UserName),

            };
            if (await _userManager.IsInRoleAsync(client, "User"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            if (await _userManager.IsInRoleAsync(client, "Admin"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            var creds = new SigningCredentials(_Key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
