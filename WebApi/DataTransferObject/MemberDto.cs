using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObject
{
    public class MemberDto
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public bool Gender { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsBanned { get; set; } = false;

    }
}
