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
        [Fact]
        public async Task AddNewItemAsIncompleteWithDueDate()
        {
            // Given
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            var dateTimeOffset = DateTimeOffset.Now;
            const string userId = "fake-000";
            const string userName = "fake@example.com";
            const string itemTitle = "Testing";

            using (var context = new ApplicationDbContext(options)) {
                var service = new TodoItemService(context);

                var fakeUser = new ApplicationUser {
                    Id = userId,
                    UserName = userName,
                };

                var todoItem = new TodoItem {
                    Title = itemTitle,
                    DueAt = dateTimeOffset
                };

                // When
                await service.AddItemAsync(todoItem, fakeUser);
            }

            // Then
            using (var context = new ApplicationDbContext(options)) {
                var itemsInDatabase = await context.Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);

                var item = await context.Items.FirstAsync();
                Assert.Equal(itemTitle, item.Title);
                Assert.False(item.IsDone);
                Assert.Equal(dateTimeOffset, item.DueAt);
            }
        }

       [Fact]
        public async Task AddNewItemAsIncompleteWithoutDueDate()
        {
            // Given
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;
            const string userId = "fake-000";
            const string userName = "fake@example.com";
            const string itemTitle = "Testing";

            using (var context = new ApplicationDbContext(options)) {
                var service = new TodoItemService(context);

                var fakeUser = new ApplicationUser {
                    Id = userId,
                    UserName = userName
                };

                var todoItem = new TodoItem {
                    Title = itemTitle
                };

                // When
                await service.AddItemAsync(todoItem, fakeUser);
            }

            // Then
            using (var context = new ApplicationDbContext(options)) {
                var itemsInDatabase = await context.Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);

                var item = await context.Items.FirstAsync();
                Assert.Equal(itemTitle, item.Title);
                Assert.False(item.IsDone);

                var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
                Assert.True(difference < TimeSpan.FromSeconds(1));
            }
        }

        [Fact]
        // EJ. MarkDoneASync method returns false if its passed an ID that doesnt exist
        public void MarkDoneAsyncMethodWithInexistantId() {
            Assert.True(false, "Not implemented");
        }

        [Fact]
        // EJ. MarkDoneAsync method returns true when it makes a valid item as complete
        public void MarkDoneAsyncMethodWithValidId() {
            Assert.True(false, "Not implemented");
        }

        [Fact]
        // EJ. GetIncompleteItemsASync method returns only the items owned by a particular user
        public void GetIncompleteItemsAsyncByUser() {
            Assert.True(false, "Not implemented");
        }
    }
}
