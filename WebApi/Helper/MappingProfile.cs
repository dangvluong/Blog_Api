using AutoMapper;
using WebApi.DataTransferObject;
using WebApi.Models;

namespace WebApi.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<Member, MemberDto>();   
            CreateMap<Comment,CommentDto>();
        }
    }
}