using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using GaucheOuDroiteBackEnd.Models;


namespace GaucheOuDroiteBackEnd.Services
{
    public class JwtTokenService(IOptions<JwtTokenSettings> p_jwtTokenSettings)
    {
        readonly JwtTokenSettings _jwtTokenSettings = p_jwtTokenSettings.Value;


        public (string Token, DateTime ExpiresAt) CreateToken(int p_userId, string p_username)
        {
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(_jwtTokenSettings.ExpiryMinutes);

            // Claims are the pieces of information we store inside the token.
            Claim[] claims =
            [
                new(ClaimTypes.NameIdentifier, p_userId.ToString()),
                new(ClaimTypes.Name, p_username),
            ];

            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(_jwtTokenSettings.Key));
            SigningCredentials credentials = new(signingKey, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = _jwtTokenSettings.Issuer,
                Audience = _jwtTokenSettings.Audience,
                SigningCredentials = credentials
            };

            JsonWebTokenHandler handler = new();
            string token = handler.CreateToken(descriptor);

            return (token, expiresAt);
        }
    }
}