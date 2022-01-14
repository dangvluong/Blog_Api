namespace WebApp.Models
{
    public class BadRequestResponse
    {
        public Dictionary<string, string[]> Errors { get; set; }
        public int Status { get; set; }
    }
}
