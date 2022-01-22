using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseController
    {
        public MemberController(RepositoryManager repository) : base(repository)
        {
           
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            IEnumerable<Member> members = await _repository.Member.GetMembers(trackChanges:false);
            if (members == null)
                return NotFound();
            return Ok(members);
        }
       
        [HttpGet("{id}")]
        [Authorize]
        public async Task<Member> GetMemberById(int id)
        {
            //Will implement only members can get data about them, or admins can view data of all members.

            //
            return await _repository.Member.GetMemberByCondition(member=> member.Id == id,trackChanges:false);
        }
        
       
    }
}
