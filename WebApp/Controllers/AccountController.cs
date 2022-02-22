using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IRepositoryManager repository, IConfiguration configuration, ILogger<AccountController> logger) : base(repository, configuration, logger)
        {
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
                    new Claim(ClaimTypes.NameIdentifier,member.Id.ToString()),
                    new Claim(ClaimTypes.Name, member.Username),
                    new Claim(ClaimTypes.GivenName, member.FullName),
                    new Claim(ClaimTypes.Email, member.Email),
                    new Claim(ClaimTypes.Gender, member.Gender ? "Nam" : "Nữ"),
                    //Save token to this claimtype
                    new Claim(ClaimTypes.Authentication, member.Token),
                };

                //Get roles of member and save to claims
                if (member.Roles != null)
                {
                    foreach (var role in member.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
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
                MailServices.SendEmailResetPassword(_configuration, result.Email,result.Token,_logger);
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
