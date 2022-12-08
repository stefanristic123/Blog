using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Like
    {
        public int Id { get; set; }
        public bool? Likes { get; set; }
        public bool? Dislikes { get; set; }
        public virtual User? User { get; set; }
        public virtual Post? Post { get; set; }
    }
}