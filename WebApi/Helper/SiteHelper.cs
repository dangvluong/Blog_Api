using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Models;

namespace WebApi.Helper
{
    public static class SiteHelper
    {
        public static byte[] HashPassword(string plaintext)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA-512");
            return algorithm.ComputeHash(Encoding.ASCII.GetBytes(plaintext));
        }
        public static string CreateToken(Member obj, string serectkey)
        {
            byte[] key = Encoding.ASCII.GetBytes(serectkey);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, obj.Id.ToString()),
                    new Claim(ClaimTypes.Name, obj.Username),
                    new Claim(ClaimTypes.Email, obj.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)                
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    };
}