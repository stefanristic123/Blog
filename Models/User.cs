﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<AppUserRole>? UserRoles { get; set; }
    }
}