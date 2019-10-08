using System;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
/*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("DataSource=todoitems.db");
        }*/

        public DbSet<TodoItem> TodoItems { set; get; }
    }
}