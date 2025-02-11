using AustralAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AustralAPI.Security
{
    public class Jwt
    {
        public string GenerateJwtToken(Cliente cliente)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bo72X2obYCjPdK7yhtmyopVPcvuk0Ccnn7rpkdvZ7tFw6wK6Jca"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, cliente.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("clienteId", cliente.Id.ToString())
    };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Tiempo de expiración del token
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}