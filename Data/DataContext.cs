using System;
using Blog.Models;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Blog.Data
{
	public class DataContext : IdentityDbContext<User, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
          
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


            modelBuilder.Entity<PostCategory>()
                   .HasKey(pc => new { pc.PostId, pc.CategoryId });
            modelBuilder.Entity<PostCategory>()
                    .HasOne(p => p.Post)
                    .WithMany(pc => pc.PostCategories)
                    .HasForeignKey(p => p.PostId);
            modelBuilder.Entity<PostCategory>()
                    .HasOne(p => p.Category)
                    .WithMany(pc => pc.PostCategories)
                    .HasForeignKey(c => c.CategoryId);
        }

    }
}

