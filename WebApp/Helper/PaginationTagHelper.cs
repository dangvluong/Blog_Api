using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace WebApp.Helper
{
    public class PaginationTagHelper : TagHelper
    {
        public int TotalPage { get; set; }
        public string Url { get; set; }
        public object CurrentPage { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "pagination-wrapper mt-4");
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"page-pagination\"><ul class=\"page-numbers\">");
            if (CurrentPage == null)
            {
                string uri = string.Format(Url, "");                
                sb.AppendFormat("<li><span aria-current=\"page\" class=\"page-numbers current\">{0}</span></li>", 1);
                for (int i = 2; i <= TotalPage; i++)
                {
                    uri = string.Format(Url, $"page={i}");
                    sb.AppendFormat("<li><a class=\"page-numbers\" href=\"{1}\">{0}</a></li>", i, uri);
                }
                if (TotalPage > 1)
                    AddNagivation(isNext: true,sb,targetPage: 2);
            }
            else
            {
                int currentPage = Convert.ToInt32(CurrentPage);
                if (currentPage > 1)
                    AddNagivation(isNext: false, sb, targetPage:currentPage - 1);
                string uri = string.Format(Url, "");
                sb.AppendFormat("<li><a class=\"page-numbers\" href=\"{1}\">{0}</a></li>", 1, uri);
                for (int i = 2; i <= TotalPage; i++)
                {
                    uri = string.Format(Url, $"page={i}");
                    if (currentPage == i)
                    {
                        sb.AppendFormat("<li><span aria-current=\"page\" class=\"page-numbers current\">{0}</span></li>", i, uri);
                    }
                    else
                    {
                        sb.AppendFormat("<li><a class=\"page-numbers\" href=\"{1}\">{0}</a></li>", i, uri);
                    }
                }
                if (currentPage < TotalPage)
                    AddNagivation(isNext: true,sb, targetPage:currentPage + 1);
            }
            output.Content.SetHtmlContent(sb.ToString());
        }

        private void AddNagivation(bool isNext, StringBuilder sb, int targetPage)
        {
            string uri = string.Format(Url, $"page={targetPage}");
            string direction = isNext ? "Next" : "Prev";
            sb.AppendFormat($"<li><a class=\"{direction} page-numbers\" href=\"{uri}\">{direction}</a></li>");
        }       
    }
}
