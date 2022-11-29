using System;
namespace Blog.Models
{
    public class Writer
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }

    }
}

