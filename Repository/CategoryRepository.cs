using System;
using Blog.Data;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Blog.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public ICollection<Post> GetPostsByCategory(int categoryId)
        {
            return _context.PostCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Post).ToList();
        }
    }
}

