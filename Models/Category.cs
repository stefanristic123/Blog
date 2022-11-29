using System;
namespace Blog.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string? Name { get; set; }
         
        public virtual ICollection<PostCategory>? PostCategories { get; set; }
    }
}

