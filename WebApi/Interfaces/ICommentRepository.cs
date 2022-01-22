﻿using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetComment(int id, bool trackChanges);
        Task<IEnumerable<CommentDto>> GetCommentsByPost(int postId, bool trackChanges);
        void UpdateComment(Comment comment);
        void AddComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}
