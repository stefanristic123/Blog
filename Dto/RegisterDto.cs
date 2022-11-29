using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Dto
{
    public class RegisterDto
    {
        [Required]
        public string  UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}

