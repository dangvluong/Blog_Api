using System.IdentityModel.Tokens.Jwt;

namespace WebApp.Services
{
    public class TokenValidator
    {
        public bool ValidateAccessToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var tokenS = jsonToken as JwtSecurityToken;
            if (tokenS.ValidTo > DateTime.UtcNow)
                return true;
            return false;
        }
    }
}
