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
    };
}