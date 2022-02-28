using WebApp.DataTransferObject;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ListPostFromCategoryViewModel : ListPostDto
    {
        public Category Category { get; set; }
    }
}
