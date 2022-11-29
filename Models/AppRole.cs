using System.Collections.Generic ;
using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class AppRole : IdentityRole<int>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}

