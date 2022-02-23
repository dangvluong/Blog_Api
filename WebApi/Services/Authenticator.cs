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

        public async Task<TokensDto> RefreshAuthentication(Member member)
        {
            TokensDto tokensDto = new TokensDto()
            {
                AccessToken = _tokenGenerator.CreateAccessToken(member),
                RefreshToken = _tokenGenerator.CreateRefreshToken()
            };            
            //Save refresh token to Db
            RefreshToken refreshTokenDto = new RefreshToken()
            {
                Token = tokensDto.RefreshToken,
                MemberId = member.Id
            };
            _repository.RefreshToken.AddToken(refreshTokenDto);
            await _repository.SaveChanges();
            return tokensDto;
        }
    }
}
