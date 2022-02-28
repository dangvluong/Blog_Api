namespace WebApp.Models.Response
{
    public class ErrorValidationResponseModel : ResponseModel
    {
        public new Dictionary<string, string[]> Errors { get; set; }        
    }
}
