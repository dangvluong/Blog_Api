﻿using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetComments(string token);
        Task<List<Comment>> GetCommentsByPostId(int id);
        Task<Comment> GetComment(int id);
        Task<int> PostComment(Comment comment, string token);
        Task<int> EditComment(Comment comment, string token);
        Task<int> DeleteComment(int id, string token);
        Task<List<Comment>> GetCommentsByMember(int id);
    }
}
