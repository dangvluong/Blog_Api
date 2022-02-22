using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Enums;
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
        public static string CreateJWToken(Member obj, string serectkey)
        {
            byte[] key = Encoding.ASCII.GetBytes(serectkey);

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

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
        public static string UploadFile(IFormFile file, UploadTypes uploadType)
        {
            string targetFolder = string.Empty;
            switch (uploadType)
            {
                case UploadTypes.Avatar:
                    targetFolder = "avatar";
                    break;
                case UploadTypes.PostThumbnail:
                    targetFolder = "thumbnails";
                    break;
                case UploadTypes.PostImages:
                    targetFolder = "postimages";
                    break;
                default:
                    break;
            }
            string folderUpload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images", targetFolder);
            if(!Directory.Exists(folderUpload))
                Directory.CreateDirectory(folderUpload);
            string newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string imageUrl = Path.Combine("images", targetFolder, newFileName);            
            string uploadLocation = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl);
            using (Stream stream = new FileStream(uploadLocation, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return imageUrl;
        }
        public static string CreateToken(int length)
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
    };
}