using System;
namespace Blog.Dto
{
    public class LikeDto
    {
        public int Id { get; set; }
        public bool? Likes { get; set; }
        public bool? Dislikes { get; set; }
        public UserDto? User { get; set; }
    }
}