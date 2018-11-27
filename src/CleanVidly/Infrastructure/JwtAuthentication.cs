using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CleanVidly.Controllers.Users;
using CleanVidly.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanVidly.Infrastructure
{
    public class JwtAuthentication
    {
        private readonly string securityKey;
        private readonly string validIssuer;
        private readonly string validAudience;

        public JwtAuthentication(IConfiguration configuration)
        {
            this.securityKey = configuration["Jwt:SecurityKey"] ?? throw new InvalidOperationException("Set the 'Jwt:SecurityKey' on appSettings");
            this.validAudience = configuration["Jwt:ValidAudience"] ?? throw new InvalidOperationException("Set the 'Jwt:SecurityKey' on appSettings");
            this.validIssuer = configuration["Jwt:ValidIssuer"] ?? throw new InvalidOperationException("Set the 'Jwt:SecurityKey' on appSettings");
        }

        public string GenerateToken(User user)
        {

            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim("name", user.Name),
                new Claim("lastname", user.Lastname),
                new Claim("joinDate", user.JoinDate.ToString("dd/MM/yyyy H:mm")),
            };

            var roles = user.UserRoles.Select(ur => new Claim("roles", ur.Role.Description));
            claims.AddRange(roles);

            return GetJwtToken(claims);
        }

        private string GetJwtToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

            var tokeOptions = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
    }
}