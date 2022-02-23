using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateAccessToken(Member obj)
        {
            //Get claims
            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.NameIdentifier, obj.Id.ToString()),
                    new Claim(ClaimTypes.Name, obj.Username),
                    new Claim(ClaimTypes.Email, obj.Email)
            };
            foreach (var role in obj.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            return this.CreateToken(_configuration.GetSection("AccessTokenSecret").ToString(), 0.25, claims);
        }

        public string CreateRefreshToken()
        {
            return this.CreateToken(_configuration.GetSection("RefreshTokenSecret").ToString(), 30);
        }

        private string CreateToken(string secretkey, double expirationMinutes, List<Claim> claims = null)
        {
            byte[] key = Encoding.ASCII.GetBytes(secretkey);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        public string CreateResetPasswordToken(int length)
        {
            string pattern = "qwertyuiopasdfghjklzxcvbnm1234567890";
            char[] arrayToken = new char[length];
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                arrayToken[i] = pattern[rand.Next(pattern.Length)];
            }
            return string.Join("", arrayToken);
        }
    }
}
