using System;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models {
    public class ApplicationDbContext : DbContext {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("DataSource=todoitems.db");
        }

        public DbSet<TodoItem> TodoItems {set; get;}
    }
}