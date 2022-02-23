using AutoMapper;
using WebApi.DataTransferObject;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class Authenticator : IAuthenticator
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repository;
        private readonly ITokenGenerator _tokenGenerator;

        public Authenticator(IMapper mapper, IRepositoryManager repository, ITokenGenerator tokenGenerator)
        {
            _mapper = mapper;
            _repository = repository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<MemberDto> Authenticate(Member member)
        {
            MemberDto memberDto = _mapper.Map<MemberDto>(member);
            memberDto.AccessToken = _tokenGenerator.CreateAccessToken(member);
            memberDto.RefreshToken = _tokenGenerator.CreateRefreshToken();
            //Save refresh token to Db
            RefreshToken refreshTokenDto = new RefreshToken()
            {
                Token = memberDto.RefreshToken,
                MemberId = memberDto.Id
            };
            _repository.RefreshToken.Create(refreshTokenDto);
            await _repository.SaveChanges();
            return memberDto;
        }
    }
}
