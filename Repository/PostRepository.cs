using System;
using AutoMapper;
using Blog.Data;
using Blog.Dto;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public PostRepository(DataContext context, IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            _photoService = photoService;
        }

        public bool CreatePost(int categoryId, Post post)
        {

            var category = _context.Categories.Where(a => a.Id == categoryId).FirstOrDefault();

            var postCategory = new PostCategory()
            {
                Category = category,
                Post = post,
            };

            _context.Add(postCategory);

            _context.Add(post);

            return Save();
        }

        public Post GetPost(int id)
        {
            return _context.Posts.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Post> GetPosts()
        {
            return _context.Posts.OrderBy(p => p.Id).Include(x => x.User ).ToList();
        }

        public Post GetPostTrimToUpper(PostDto postCreate)
        {
            return GetPosts().Where(c => c.Title.Trim().ToUpper() == postCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool UpdatePost(Post post)
        {
            _context.Update(post);
            return Save();
        }
    }

   
}

