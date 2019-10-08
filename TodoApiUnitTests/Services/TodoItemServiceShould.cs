using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApi.Mappings;
using TodoApi.Models;

namespace TodoApi.UnitTests.Services
{
    public partial class TodoItemServiceShould
    {
        private const long _DefaultItemId = 1;
        private const string _DefaultDatabaseName = "TodoItemUnitTestDb";
        private const string _DefaultItemName = "TodoItem";
        private const string _DefaultItemDescription = "Esta es la descripcion del TodoItem";
        private const long _DefaultItemOrder = 0;

        private static TodoItem CreateFakeTodoItem(long itemId = _DefaultItemId, string itemName = _DefaultItemName, string itemDescription = _DefaultItemDescription, bool completed = false, long order = _DefaultItemOrder, DateTimeOffset dateTimeOffset = default(DateTimeOffset)) => new TodoItem
        {
            Id = itemId,
            Name = $"{itemName} {itemId}",
            Description = $"{itemDescription} {itemId}",
            IsComplete = completed,
            Order = order,
            DueAt = dateTimeOffset,
        };

        private static DbContextOptions<ApplicationDbContext> GetInMemoryOptions() {
            return new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: _DefaultDatabaseName).Options;
        }

        private static IMapper GetMapper() {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SimpleMapping());
            }).CreateMapper();
        }

        private static void ClearDataBase(DbContextOptions<ApplicationDbContext> options) {
            using (var context = new ApplicationDbContext(options)) {
                context.Database.EnsureDeleted();
            }
        }
    }
}