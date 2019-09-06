using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Data;

namespace AspNetCoreTodo.Test
{
    public class TodoItemServiceShould
    {
        private const string _DefaultUserId = "fake-000";
        private const string _DefaultUserName = "fake@example.com";
        private const string _DefaultItemTitle = "Testing";

        private static void ClearDataBase(DbContextOptions<ApplicationDbContext> options) {
            using (var context = new ApplicationDbContext(options)) {
                context.Database.EnsureDeleted();
            }
        }

        private static TodoItem CreateTodoItem(string userId, DateTimeOffset dateTimeOffset = default(DateTimeOffset), string itemTitle = _DefaultItemTitle, bool completed = false)
        {
            return new TodoItem
            {
                Title = itemTitle,
                DueAt = dateTimeOffset,
                UserId = userId,
                IsDone = completed
            };
        }

        private static ApplicationUser CreateFakeUser(string userId = _DefaultUserId, string userName = _DefaultUserName)
        {
            return new ApplicationUser
            {
                Id = userId,
                UserName = userName,
            };
        }

        [Fact]
        public async Task AddNewItemAsIncompleteWithDueDate()
        {
            // Given
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            var dateTimeOffset = DateTimeOffset.Now;

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                ApplicationUser fakeUser = CreateFakeUser();
                TodoItem todoItem = CreateTodoItem(userId:fakeUser.Id, dateTimeOffset);

                // When
                await service.AddItemAsync(todoItem, fakeUser);
            }

            // Then
            using (var context = new ApplicationDbContext(options)) {
                var itemsInDatabase = await context.Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);

                var item = await context.Items.FirstAsync();
                Assert.Equal(_DefaultItemTitle, item.Title);
                Assert.False(item.IsDone);
                Assert.Equal(dateTimeOffset, item.DueAt);
            }

            ClearDataBase(options);
        }

        [Fact]
        public async Task AddNewItemAsIncompleteWithoutDueDate()
        {
            // Given
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            using (var context = new ApplicationDbContext(options)) {
                var service = new TodoItemService(context);
                var fakeUser = CreateFakeUser();
                var todoItem = CreateTodoItem(userId:fakeUser.Id);

                // When
                await service.AddItemAsync(todoItem, fakeUser);
            }

            // Then
            using (var context = new ApplicationDbContext(options)) {
                var itemsInDatabase = await context.Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);

                var item = await context.Items.FirstAsync();
                Assert.Equal(_DefaultItemTitle, item.Title);
                Assert.False(item.IsDone);

                var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
                Assert.True(difference < TimeSpan.FromSeconds(1), $"La diferencia fue de {difference} segundos.");
            }

            ClearDataBase(options);
        }

        [Fact]
        public async Task MarkDoneAsyncMethodWithInexistantId() {
            // Given
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            var dateTimeOffset = DateTimeOffset.Now;

            using (var context = new ApplicationDbContext(options)) {
                var service = new TodoItemService(context);
                var fakeUser = CreateFakeUser();

                // When
                bool result = await service.MarkDoneAsync(Guid.NewGuid(), fakeUser);

                // Then
                Assert.False(result);
            }

            ClearDataBase(options);
        }

        [Fact]
        public async Task MarkDoneAsyncMethodWithValidId() {
            // Given
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            var dateTimeOffset = DateTimeOffset.Now;

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var fakeUser = CreateFakeUser();
                var todoItem = CreateTodoItem(userId:fakeUser.Id, dateTimeOffset);

                await service.AddItemAsync(todoItem, fakeUser);
            }

            using (var context = new ApplicationDbContext(options)) {
                var service = new TodoItemService(context);
                var fakeUser = CreateFakeUser();
                var itemsInDatabase = await context.Items.CountAsync();
                var item = await context.Items.FirstAsync();

                bool result = await service.MarkDoneAsync(item.Id, fakeUser);
                Assert.True(result);
            }

            ClearDataBase(options);
        }

        [Fact]
        public async Task GetIncompleteItemsAsyncByUser() {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            DateTime dueAt = DateTime.Today.AddDays(5);
            ApplicationUser userWithIncompletedItems = CreateFakeUser();
            ApplicationUser otherUser = CreateFakeUser("fake-001", "fake1@example.com");
            TodoItem todoItemCompleted = CreateTodoItem(userWithIncompletedItems.Id, dueAt.AddDays(-10), "TodoItemCompleted", true);
            TodoItem todoItemIncompleted = CreateTodoItem(userWithIncompletedItems.Id, dueAt, "TodoItemIncompleted");
            TodoItem todoItemFromOtherUser = CreateTodoItem(otherUser.Id, dueAt.AddDays(5), "TodoItemFromOtherUser");

            using (var context = new ApplicationDbContext(options)) {
                await context.Items.AddAsync(todoItemCompleted);
                await context.Items.AddAsync(todoItemIncompleted);
                await context.Items.AddAsync(todoItemFromOtherUser);

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options)) {
                var service = new TodoItemService(context);
                var todoItemsIncompletedForUser = await service.GetIncompleteItemsAsync(userWithIncompletedItems);
                Assert.True(todoItemsIncompletedForUser.Length == 1);

                var item = todoItemsIncompletedForUser[0];
                Assert.Equal(todoItemIncompleted.Id, item.Id);
                Assert.Equal(todoItemIncompleted.DueAt, item.DueAt);
                Assert.False(item.IsDone);
                Assert.Equal(todoItemIncompleted.Title, item.Title);
                Assert.Equal(todoItemIncompleted.UserId, userWithIncompletedItems.Id);
            }

            ClearDataBase(options);
        }
    }
}
