using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using WebApp.Data;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AccountController : BaseController
    {
        private IMailServices _mailServices;
        public AccountController(IRepositoryManager repository, IConfiguration configuration, IMailServices mailServices,ILogger<AccountController> logger) : base(repository, configuration, logger)
        {
            _mailServices = mailServices;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "")
        {
            if (!ModelState.IsValid)
                return View();
            Member member = await _repository.Auth.Login(model);
            if (member != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(System.Security.Claims.ClaimTypes.NameIdentifier,member.Id.ToString()),
                    new Claim(System.Security.Claims.ClaimTypes.Name, member.Username),
                    new Claim(System.Security.Claims.ClaimTypes.GivenName, member.FullName),
                    new Claim(System.Security.Claims.ClaimTypes.Email, member.Email),
                    new Claim(System.Security.Claims.ClaimTypes.Gender, member.Gender ? "Nam" : "Nữ"),
                    //Save token to this claimtype
                    new Claim(Data.ClaimTypes.AccessToken, member.AccessToken),
                    new Claim(Data.ClaimTypes.RefreshToken, member.RefreshToken),
                };

                //Get roles of member and save to claims
                if (member.Roles != null)
                {
                    foreach (var role in member.Roles)
                    {
                        claims.Add(new Claim(System.Security.Claims.ClaimTypes.Role, role.Name));
                    }
                }
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    IsPersistent = model.Remember
                };
                await HttpContext.SignInAsync(principal, properties);
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return Redirect("/");

            }
            ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác");
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View();
            await _repository.Auth.Register(model);
            return RedirectToAction(nameof(Login));
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //string token = User.FindFirst(Data.ClaimTypes.AccessToken).Value;
            //Remove refresh tokens at api server
            await _repository.Auth.Logout(AccessToken);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(nameof(Login));
        }
        public IActionResult Denied()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();
            var httpContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("",email)
            });
            ResetPasswordModel result = await _repository.Auth.ForgotPassword(httpContent);
            if (result != null)
            {
                //Skip waiting for email sending
                _mailServices.SendEmailResetPassword(result.Email,result.Token,_logger);
            }
            //Add notification 
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult ResetPassword(ResetPasswordModel obj)
        {
            if (obj == null)
                return BadRequest();
            ModelState.Clear();
            return View(obj);
        }
        [HttpPost]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordModel obj)
        {
            if (!ModelState.IsValid)
                return View();
            int result = await _repository.Auth.ResetPassword(obj);
            return RedirectToAction(nameof(Login));
        }
    }
}
