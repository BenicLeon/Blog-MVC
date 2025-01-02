using System;
using System.Collections.Generic;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data;

public partial class BlogsContext : DbContext
{
    public BlogsContext()
    {
    }

    public BlogsContext(DbContextOptions<BlogsContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<BlogPost>()
       .ToTable("blog_posts");

        modelBuilder.Entity<BlogPost>()
       .Property(bp => bp.CreatedAt)
       .HasColumnName("created_at");  

        modelBuilder.Entity<BlogPost>()
            .Property(bp => bp.UpdatedAt)
            .HasColumnName("updated_at");

        modelBuilder.Entity<BlogPost>()
            .Property(bp => bp.UserId)
            .HasColumnName("user_id");

       
        modelBuilder.Entity<Comment>()
            .Property(c => c.CreatedAt)
            .HasColumnName("created_at"); 

        modelBuilder.Entity<Comment>()
            .Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        modelBuilder.Entity<Comment>()
            .Property(c => c.UserId)
            .HasColumnName("user_id");

        modelBuilder.Entity<Comment>()
            .Property(c => c.BlogPostId)
            .HasColumnName("blogpost_id");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
