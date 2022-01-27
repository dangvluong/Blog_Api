namespace WebApi.DataTransferObject
{
    public class UpdateRolesOfMemberDto
    {
        public int MemberId { get; set; }
        public int[] RoleIds { get; set; }
    }
}
