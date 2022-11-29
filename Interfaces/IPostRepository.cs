using System;
using Blog.Dto;
using Blog.Models;

namespace Blog.Interfaces
{
    public interface IPostRepository
    {
        ICollection<Post> GetPosts();

        Post GetPost(int id);

        bool CreatePost(int categoryId, Post post);

        Post GetPostTrimToUpper(PostDto PostCreate);

        bool UpdatePost(Post post);
        bool Save();

        Task<bool> SaveAllAsync();
    }
}

