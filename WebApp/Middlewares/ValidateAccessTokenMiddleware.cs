using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using WebApp.Data;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Services;

namespace WebApp.Middlewares
{
    public class ValidateAccessTokenMiddleware
    {
        private readonly RequestDelegate _next;       
        public ValidateAccessTokenMiddleware(RequestDelegate next)
        {
            _next = next;        
        }

        public async Task Invoke(HttpContext context, TokenValidator tokenValidator, IRepositoryManager repository)
        {            
            if(context.User.Identity.IsAuthenticated){
                var accessToken = context.User.FindFirstValue(Data.ClaimTypes.AccessToken);               
                bool isValid = tokenValidator.ValidateAccessToken(accessToken);
                if (!isValid)
                {
                    var refreshToken = context.User.FindFirstValue(Data.ClaimTypes.RefreshToken);                    
                    TokensDto tokensDto = await repository.Auth.RefreshTokens(new TokensDto { RefreshToken = refreshToken});
                    if(tokensDto != null)
                    {
                        var identity = context.User.Identity as ClaimsIdentity;
                        if (identity != null)
                        {
                            var claimsKey = new string[] { Data.ClaimTypes.AccessToken, Data.ClaimTypes.RefreshToken };
                            foreach (var key in claimsKey)
                            {
                                var existingClaim = identity.FindFirst(key);
                                if (existingClaim != null)
                                    identity.RemoveClaim(existingClaim);
                            }                           
                            identity.AddClaim(new Claim(Data.ClaimTypes.AccessToken, tokensDto.AccessToken));
                            identity.AddClaim(new Claim(Data.ClaimTypes.RefreshToken, tokensDto.RefreshToken));
                            context.User.AddIdentity(identity);
                            //Persist updated claims
                            await context.SignInAsync(new ClaimsPrincipal(identity));
                        }
                    }
                    else
                    {
                        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }                
            }
            await _next(context);
        }
    }
}
