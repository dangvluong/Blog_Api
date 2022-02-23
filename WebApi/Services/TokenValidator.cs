using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class TokenValidator : ITokenValidator
    {
        private readonly IConfiguration _configuration;

        public TokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("RefreshTokenSecret").ToString())),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true; 
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
