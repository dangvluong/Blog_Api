﻿using WebApp.DataTransferObject;
using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IAuthRepository
    {
        Task<Member> Login(LoginModel model);
        Task<int> Register(RegisterModel model);       
        Task<BadRequestResponse> ChangePassword(ChangePasswordModel obj, string token);
        Task<ResetPasswordDto> ForgotPassword(HttpContent httpContent);
        Task<int> ResetPassword(ResetPasswordDto obj);
    }
}
