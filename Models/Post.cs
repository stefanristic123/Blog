using System;
using Blog.Dto;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
        
        public virtual User? User { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        public virtual ICollection<PostCategory>? PostCategories { get; set; }

        public virtual List<Photo> Photos { get; set; } = new();

    }
}

   