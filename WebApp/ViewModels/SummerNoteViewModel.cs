namespace WebApp.ViewModels
{
    public class SummerNoteViewModel
    {
        public string TargetElementId { get; set; }
        public SummerNoteViewModel(string id)
        {
            TargetElementId = id;
        }
    }
}
