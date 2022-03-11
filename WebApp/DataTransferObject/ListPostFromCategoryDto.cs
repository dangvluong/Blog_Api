using WebApp.DataTransferObject;
using WebApp.Models;

namespace WebApp.DataTransferObject
{
    public class ListPostFromCategoryDto : ListPostDto
    {
        public Category Category { get; set; }
    }
}
