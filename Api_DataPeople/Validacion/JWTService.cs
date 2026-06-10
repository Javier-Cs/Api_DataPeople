using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api_DataPeople.Model;
using Microsoft.IdentityModel.Tokens;

namespace Api_DataPeople.Validacion
{
    public class JWTService
    {
        IConfiguration _config;
        public JWTService(IConfiguration configuration) {
            _config = configuration;
        }

        public string GenerarToken(Usuario user, DateTime expiracion) {

            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, user.id_usuario.ToString()),
                new Claim(ClaimTypes.Name, user.nombre),
                new Claim(ClaimTypes.Role, user.rol),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                ),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64
                )
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiracion,
                signingCredentials:cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
