using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApi.UnitTests.Services
{
    public partial class TodoItemServiceShould
    {
        [Fact]
        public async Task GetTodoItemAsync_WithValidId_ShouldReturnDTO()
        {
            // Arrange
            var expectedTodoItem = CreateFakeTodoItem();
            var options = GetInMemoryOptions();
            var mapper = GetMapper();

            ClearDataBase(options);

            using (var context = new ApplicationDbContext(options))
            {
                await context.TodoItems.AddAsync(expectedTodoItem);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);
                var todoItem  = await service.GetTodoItemAsync(expectedTodoItem.Id);

                // Assert
                Assert.NotNull(todoItem);
                Assert.Equal(expectedTodoItem.Name, todoItem.Name);
                Assert.Equal(expectedTodoItem.Description, todoItem.Description);
                Assert.Equal(expectedTodoItem.IsComplete, todoItem.IsComplete);
                Assert.Equal(expectedTodoItem.DueAt, todoItem.DueAt);
                Assert.Equal(expectedTodoItem.Order, todoItem.Order);
            }

            ClearDataBase(options);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(2)]
        public async Task GetTodoItemAsync_WithInvalidIdAndExistingElement_ShouldThrowException(long id)
        {
            // Arrange
            var expectedTodoItem = CreateFakeTodoItem();
            var options = GetInMemoryOptions();
            var mapper = GetMapper();

            ClearDataBase(options);

            using (var context = new ApplicationDbContext(options))
            {
                await context.TodoItems.AddAsync(expectedTodoItem);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);

                // Act / Assert
                await Assert.ThrowsAsync<ArgumentException>("id", () => service.GetTodoItemAsync(id));
            }

            ClearDataBase(options);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetTodoItemAsync_WithInvalidIdAndEmptyList_ShouldThrowException(long id)
        {
            // Arrange
            var options = GetInMemoryOptions();
            var mapper = GetMapper();

            ClearDataBase(options);

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);

                // Act / Assert
                await Assert.ThrowsAsync<ArgumentException>("id", () => service.GetTodoItemAsync(id));
            }

            ClearDataBase(options);
        }

        [Fact]
        public async Task GetTodoItemsAsync_WithNoElements_ShouldReturnEmpty()
        {
            // Arrange
            var options = GetInMemoryOptions();
            var mapper = GetMapper();

            ClearDataBase(options);

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);
                var todoItems = await service.GetTodoItemsAsync();

                // Assert
                Assert.NotNull(todoItems);
                Assert.Empty(todoItems);
            }

            ClearDataBase(options);
        }

        [Fact]
        public async Task GetTodoItemsAsync_WithElements_ShouldReturnElements()
        {
           // Arrange
            var expectedTodoItems = new[] {
                CreateFakeTodoItem(1),
                CreateFakeTodoItem(2),
                CreateFakeTodoItem(3)
            };
            var options = GetInMemoryOptions();
            var mapper = GetMapper();

            ClearDataBase(options);

            using (var context = new ApplicationDbContext(options))
            {
                List<Task> tasks = new List<Task>();

                foreach (var expectedTodoItem in expectedTodoItems) {
                    tasks.Add(context.TodoItems.AddAsync(expectedTodoItem));
                }

                await Task.WhenAll(tasks);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);
                var todoItems = await service.GetTodoItemsAsync();

                // Assert
                Assert.NotNull(todoItems);
                Assert.True(todoItems.Count == 3);

                for (int index = 0; index < todoItems.Count; index++) {
                    Assert.Equal($"TodoItem {index + 1}", todoItems[index].Name);
                }
            }

            ClearDataBase(options);
        }
    }
}