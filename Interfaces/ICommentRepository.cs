using System;
using Blog.Models;

namespace Blog.Interfaces
{
    public interface ICommentRepository
    {
        ICollection<Comment> GetComments();
        Comment GetComment(int id);
        ICollection<Comment> GetCommentsForPost(int postId);
        bool CommentExists(int id);
        bool CreateComment(Comment comment);
        bool UpdateComment(Comment comment);
        bool DeleteComment(Comment comment);
        bool Save();
    }
}

