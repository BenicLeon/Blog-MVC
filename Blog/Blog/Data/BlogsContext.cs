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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-KDHUHUT;Initial Catalog=blogs;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
