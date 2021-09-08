using System;
using Microsoft.EntityFrameworkCore;
using TakeoutSystem.Models;

namespace TakeoutSystem.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Order { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Order>().ToTable("Orders");
        }

    }
}