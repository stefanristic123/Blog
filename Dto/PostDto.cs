using System;
using Blog.Models;

namespace Blog.Dto
{
    public class PostDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public WriterDto?  Writer { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; } 

        public virtual ICollection<Photo>? Photos { get; set; } 

    }
}

 