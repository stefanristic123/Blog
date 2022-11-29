using System;
using Blog.Data;
using Blog.Interfaces;
using Blog.Models;

namespace Blog.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public bool CommentExists(int id)
        {
            return _context.Users.Any(c => c.Id == id);
        }

        public bool CreateComment(Comment comment)
        {
            _context.Add(comment);
            return Save();
        }

        public bool DeleteComment(Comment comment)
        {
            _context.Remove(comment);
            return Save();
        }

        public Comment GetComment(int id)
        {
            return _context.Comments.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Comment> GetComments()
        {
            return _context.Comments.ToList();
        }

        public ICollection<Comment> GetCommentsForPost(int postId)
        {

            return (ICollection<Comment>)_context.Posts.Where(e => e.Id == postId).Select(c => c.Comments).ToList();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateComment(Comment comment)
        {
            _context.Update(comment);
            return Save();
        }
    }
}

