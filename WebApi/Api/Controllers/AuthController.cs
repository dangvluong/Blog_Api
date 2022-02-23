using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DataTransferObject;
using WebApi.Helper;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenValidator _tokenValidator;
        private readonly IAuthenticator _authenticator;
        private ILogger<AuthController> _logger;
        public AuthController(IRepositoryManager repository, IMapper mapper, IConfiguration configuration, ITokenGenerator tokenGenerator, ITokenValidator tokenValidator,IAuthenticator authenticator, ILogger<AuthController> logger) : base(repository, mapper)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
            _authenticator = authenticator;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Member member = new Member
            {
                Username = model.Username,
                Password = SiteHelper.HashPassword(model.Password),
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                FullName = model.FullName,
                DateCreate = DateTime.Now,
                AvatarUrl = DefaultValue.Avatar,
                AboutMe = DefaultValue.About
            };
            member.Roles = new List<Role>();
            var role = await _repository.Role.GetRoleByName("Member", trackChanges: true);
            member.Roles.Add(role);
            _repository.Member.AddMember(member);
            await _repository.SaveChanges();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<MemberDto>> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //Member member = await _repository.Member.Login(loginModel);
            Member member = await _repository.Member.GetMemberByCondition(member =>
                member.Username == loginModel.Username && member.Password == SiteHelper.HashPassword(loginModel.Password)
            , trackChanges: false);

            if (member != null)
            {
                MemberDto memberDto = _mapper.Map<MemberDto>(member);
                memberDto.AccessToken = _tokenGenerator.CreateAccessToken(member);
                memberDto.RefreshToken = _tokenGenerator.CreateRefreshToken();
                RefreshToken refreshToken = new RefreshToken()
                {
                    MemberId = member.Id,
                    Token = memberDto.RefreshToken
                };
                _repository.RefreshToken.AddToken(refreshToken);
                await _repository.SaveChanges();
                return Ok(memberDto);
            }
            return BadRequest();
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel obj)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Member member = await _repository.Member.GetMemberByCondition(member =>
                   member.Id == memberId, trackChanges: true
               );
            if (member == null)
                return BadRequest();
            if (!member.Password.SequenceEqual(SiteHelper.HashPassword(obj.OldPassword)))
            {
                ModelState.AddModelError(nameof(obj.OldPassword), "Mật khẩu cũ không đúng");
                return ValidationProblem(ModelState);
            }
            member.Password = SiteHelper.HashPassword(obj.NewPassword);
            await _repository.SaveChanges();
            return NoContent();
        }
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromForm]string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();
            Member member = await _repository.Member.GetMemberByCondition(m => m.Email == email, trackChanges: true);
            if (member == null)
            {
                ModelState.AddModelError(nameof(member.Email), "Email không tồn tại");
                return ValidationProblem(ModelState);
            }
            if (member.IsBanned)
            {
                ModelState.AddModelError(nameof(member.Username), "Tài khoản của bạn đã bị khóa.");
                return ValidationProblem(ModelState);              
            }
            else
            {
                member.ResetPasswordToken = _tokenGenerator.CreateResetPasswordToken(32);
                await _repository.SaveChanges();
                ResetPasswordDto resetPasswordDto = new ResetPasswordDto
                {
                    Token = member.ResetPasswordToken,
                    Email = email,
                };
                return Ok(resetPasswordDto);
            }

        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto obj)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Member member = await _repository.Member.GetMemberByCondition(m => m.Email == obj.Email && m.ResetPasswordToken == obj.Token, trackChanges: true);
            if (member == null)
                return BadRequest();
            member.Password = SiteHelper.HashPassword(obj.NewPassword);
            member.ResetPasswordToken = null;
            await _repository.SaveChanges();
            return NoContent();
        }

        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken(TokensDto tokensDto)
        {            
            if (string.IsNullOrEmpty(tokensDto?.RefreshToken))
                return BadRequest();
            bool isValidRefreshToken = _tokenValidator.ValidateRefreshToken(tokensDto.RefreshToken);
            if (!isValidRefreshToken)
                return BadRequest("Invalid refresh token.");
            RefreshToken refreshToken = await _repository.RefreshToken.GetByToken(tokensDto.RefreshToken, trackChanges:true);
            if (refreshToken == null)
            {
                _logger.LogError("Khong tim thay refresh token trong Db");
                return NotFound("Invalid refresh token");
            }                
            _repository.RefreshToken.DeleteToken(refreshToken);   
            await _repository.SaveChanges();
            Member member = await _repository.Member.GetMemberByCondition(m => m.Id == refreshToken.MemberId, trackChanges: false);
            if (member == null)
                return NotFound("Member not found");
            TokensDto newTokens  = await _authenticator.RefreshAuthentication(member);
            _logger.LogInformation("Refreshed token");
            return Ok(newTokens);
        }
        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var tokens = await _repository.RefreshToken.GetByMember(memberId, trackChanges: true);
            if(tokens != null)
            {
                _repository.RefreshToken.DeleteTokens(tokens);
                await _repository.SaveChanges();
            }            
            return NoContent();
        }
    }

}

