namespace WebApp.Models.Response
{
    public abstract class ResponseModel
    {        
        public int Status { get; set; }
        public object Data { get; set; }
        public object Errors { get; set; }
    }
}
