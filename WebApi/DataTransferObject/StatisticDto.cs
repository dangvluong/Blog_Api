using WebApi.Models;

namespace WebApi.DataTransferObject
{
    public class StatisticDto
    {        
        public int TotalPost { get; set; }
        public int TotalUnapprovedPost { get; set; }
        public int NewPosts { get; set; }
        public int TotalMember { get; set; }
        public int NewMember { get; set; }
        public int TotalComment { get; set; }

    }
}
