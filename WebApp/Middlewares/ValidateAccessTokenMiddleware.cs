using System.Security.Claims;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Services;

namespace WebApp.Middlewares
{
    public class ValidateAccessTokenMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly TokenValidator _tokenValidator;        
        public ValidateAccessTokenMiddleware(RequestDelegate next)
        {
            _next = next;
            //_tokenValidator = tokenValidator;
        }

        public async Task Invoke(HttpContext context, TokenValidator tokenValidator, IRepositoryManager repository)
        {
            if(context.User.Identity.IsAuthenticated){
                var accessToken = context.User.FindFirstValue("AccessToken");
                Console.WriteLine(accessToken);
                bool isValid = tokenValidator.ValidateAccessToken(accessToken);
                if (!isValid)
                {
                    var refreshToken = context.User.FindFirstValue("RefreshToken");
                    TokensDto tokensDto = await repository.Auth.RefreshTokens(new TokensDto { RefreshToken = refreshToken});
                    if(tokensDto != null)
                    {
                        var identity = context.User.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                            var claimsKey = new string[] {"AccessToken", "RefreshToken" };
                            foreach (var key in claimsKey)
                            {
                                var existingClaim = identity.FindFirst(key);
                                if (existingClaim != null)
                                    identity.RemoveClaim(existingClaim);
                            }   
                            var newClaims = new List<Claim>()
                            {
                                new Claim("AccessToken",tokensDto.AccessToken),
                                new Claim("RefreshToken",tokensDto.RefreshToken),
                            };
                            var newIdentities = new ClaimsIdentity(newClaims);
                            context.User.AddIdentity(newIdentities);
                        }
                    }
                }                
            }
            await _next(context);
        }
    }
}
