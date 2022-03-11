using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Controllers
{
    public class AccountController : BaseController
    {
        private IMailServices _mailServices;
        public AccountController(IRepositoryManager repository, IConfiguration configuration, IMailServices mailServices, ILogger<AccountController> logger) : base(repository, configuration, logger)
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
            ResponseModel response = await _repository.Auth.Login(model);
            if (response is SuccessResponseModel)
            {
                Member member = response.Data as Member;
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,member.Id.ToString()),
                    new Claim(ClaimTypes.Name, member.Username),
                    new Claim(ClaimTypes.GivenName, member.FullName),
                    new Claim(ClaimTypes.Email, member.Email),
                    new Claim(ClaimTypes.Gender, member.Gender ? "Nam" : "Nữ"),
                    //Save token to this claimtype
                    new Claim(Data.ClaimTypes.AccessToken, member.AccessToken),
                    new Claim(Data.ClaimTypes.RefreshToken, member.RefreshToken),
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
                PushNotification(new NotificationOptions()
                {
                    Type = "success",
                    Message = "Đăng nhập thành công"
                });
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return Redirect("/");
            }
            HandleErrors(response);
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
            ResponseModel response = await _repository.Auth.Register(model);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions()
                {
                    Type = "success",
                    Message = "Đăng kí tài khoản thành công"
                });
                return RedirectToAction(nameof(Login));
            }
            HandleErrors(response);
            return View();

        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //Remove refresh tokens at api server
            await _repository.Auth.Logout(AccessToken);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            PushNotification(new NotificationOptions()
            {
                Type = "success",
                Message = "Đăng xuất thành công"
            });
            return RedirectToAction(nameof(Login));
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel obj)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            ResponseModel response = await _repository.Auth.ForgotPassword(obj);
            if (response is SuccessResponseModel)
            {
                ResetPasswordModel resetPasswordInfo = (ResetPasswordModel)response.Data;
                _mailServices.SendEmailResetPassword(resetPasswordInfo.Email, resetPasswordInfo.Token, _logger);
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đường dẫn chứa liên kết đặt lại mật khẩu đã được gửi đến email của bạn"
                });
            }
            else
                HandleErrors(response);
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
            ResponseModel response = await _repository.Auth.ResetPassword(obj);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật mật khẩu mới"
                });
                return RedirectToAction(nameof(Login));
            }
            HandleErrors(response);
            return View();
        }
    }
}
