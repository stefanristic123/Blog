using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; }
        public virtual AppRole Role { get; set; } 
    }
}

