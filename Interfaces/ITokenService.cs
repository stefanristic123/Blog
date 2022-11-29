using System;
using Blog.Models;

namespace Blog.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}

