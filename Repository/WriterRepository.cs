using System;
using Blog.Data;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
    public class WriterRepository : IWriterRepository
    {
        private readonly DataContext _context;

        public WriterRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Writer> GetWriters()
        {
            return _context.Writers.OrderBy(p => p.Id).Include(e => e.Posts).ToList();
        }

        public Writer GetWriter(int id)
        {
            return _context.Writers.Where(p => p.Id == id).Include(e => e.Posts).FirstOrDefault();

        }

        public bool CreateWriter(Writer writer)
        {
            _context.Add(writer);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

