namespace WebApi.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public int MemberId { get; set; }
    }
}
