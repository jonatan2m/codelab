using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using M220N.Models;
using Microsoft.IdentityModel.Tokens;

namespace M220N
{
    public class JwtAuthentication
    {
        public string SecurityKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Convert.FromBase64String(SecurityKey));
        public SigningCredentials SigningCredentials => new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        public string GenerateToken(User user)
        {
            var token = new JwtSecurityToken(
                issuer: this.ValidIssuer,
                audience: this.ValidAudience,
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                },
                expires: DateTime.UtcNow.AddDays(30),
                notBefore: DateTime.UtcNow,
                signingCredentials: this.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
