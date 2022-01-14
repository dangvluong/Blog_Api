﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IRepositoryManager repository) : base(repository)
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
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel obj)
        {
            if (!ModelState.IsValid)
                return View();
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            //bool isOldPasswordValid = await _repository.Auth.CheckOldPasswordValid(changePasswordModel.OldPassword, token);
            //if (!isOldPasswordValid)
            //{
            //    ModelState.AddModelError(string.Empty, "Mật khẩu cũ không đúng");
            //    return View();
            //}                
            BadRequestResponse response = await _repository.Auth.ChangePassword(obj, token);
            if (response == null)
                return RedirectToAction(nameof(Logout));
            foreach (var errorMessage in response.Errors)
            {
                foreach (var error in errorMessage.Value)
                {
                    ModelState.AddModelError(errorMessage.Key, error);
                }
            }
            return View();
        }
    }
}