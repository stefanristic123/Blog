using System;
using Blog.Models;

namespace Blog.Interfaces
{
    public interface IWriterRepository
    {
        ICollection<Writer> GetWriters();

        Writer GetWriter(int id);

        bool CreateWriter(Writer writer);

        bool Save();

    }
}

