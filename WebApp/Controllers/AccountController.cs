using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using WebApp.Data;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;
using WebApp.Services;

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
                PushNotification(new NotificationOption()
                {
                    Type = "success",
                    Message = "Đăng nhập thành công"
                });
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
            PushNotification(new NotificationOption()
            {
                Type = "success",
                Message = "Đăng kí tài khoản thành công"
            });
            return RedirectToAction(nameof(Login));
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //Remove refresh tokens at api server
            await _repository.Auth.Logout(AccessToken);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            PushNotification(new NotificationOption()
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
                PushNotification(new NotificationOption
                {
                    Type = "success",
                    Message = "Đường dẫn chứa liên kết đặt lại mật khẩu đã được gửi đến email của bạn"
                });
                return RedirectToAction(nameof(Login));
            }
            else if (response is ErrorMessageResponseModel)
            {
                PushNotification(new NotificationOption
                {
                    Type = "error",
                    Message = (string)response.Data
                });
                return View();
            }
            else
            {
                PushError((Dictionary<string, string[]>)response.Data);
                return View();
            }
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
                PushNotification(new NotificationOption
                {
                    Type = "success",
                    Message = "Đã cập nhật mật khẩu mới"
                });
                return RedirectToAction(nameof(Login));
            }
            else if (response is ErrorMessageResponseModel)
            {
                PushNotification(new NotificationOption
                {
                    Type = "error",
                    Message = (string)response.Data
                });
                return View();
            }
            else
            {
                PushError((Dictionary<string, string[]>)response.Data);
                return View();
            }
        }
    }
}
