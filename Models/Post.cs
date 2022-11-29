using System;
namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public virtual Writer? Writer { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        public virtual ICollection<PostCategory>? PostCategories { get; set; }

        public virtual List<Photo> Photos { get; set; } = new();

    }
}

   