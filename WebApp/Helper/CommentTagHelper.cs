using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using WebApp.Models;

namespace WebApp.Helper
{
    public class CommentTagHelper : TagHelper
    {
        public List<Comment> ListComment { get; set; }
        public bool isAuthenticated { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"comments mt-5\">");
            sb.Append($"<h4 class=\"side-title\">Comments({ListComment.Count})</h4>");
            foreach (Comment comment in ListComment)
            {
                if (comment.CommentParentId == null)
                {
                    int depthLevel = 1;
                    ParseContent(sb, comment, depthLevel);
                }
            }
            sb.Append("</div>");
            output.Content.SetHtmlContent(sb.ToString());
        }

        private void ParseContent(StringBuilder sb, Comment comment, int depthLevel)
        {
            if (depthLevel == 1)
                sb.Append("<div class=\"media\">");
            else
                sb.Append("<div class=\"media second mt-4 p-0 pt-2\">");
            sb.Append("<div class=\"img-circle\">");
            sb.Append("<img src=\"./post and comments_files/c2.jpg\" class=\"img-fluid\" alt=\"...\">");
            sb.Append("</div>");
            sb.Append("<div class=\"media-body\">");
            sb.Append("<ul class=\"time-rply mb-2\">");
            sb.Append("<li>");
            sb.Append($"<a href=\"#\" class=\"name mt-0 mb-2 d-block\">{comment.AuthorName}</a>");
            sb.Append($"{comment.DateCreate.ToShortDateString()} - {comment.DateCreate.ToShortTimeString()}");
            sb.Append("</li>");
            if (isAuthenticated && depthLevel < 5)
            {
                sb.Append("<li class=\"reply-last\">");
                sb.Append($"<a href=\"#reply\" class=\"reply reply-comment\" data-comment-id=\"{comment.Id}\">");
                sb.Append("Reply");
                sb.Append("</a>");
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            sb.Append("<p>");
            sb.Append(comment.Content);
            sb.Append("</p>");
            depthLevel++;
            foreach (var childComment in ListComment)
            {
                if (childComment.CommentParentId == comment.Id)
                {
                    ParseContent(sb, childComment, depthLevel);
                }
            }
            sb.Append("</div>");
            sb.Append("</div>");
        }
    }
}
