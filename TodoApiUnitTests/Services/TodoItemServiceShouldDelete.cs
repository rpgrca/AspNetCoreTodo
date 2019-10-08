using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApi.UnitTests.Services
{
    public partial class TodoItemServiceShould
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task DeleteTodoItemAsync_WithInvalidIdAndEmptyList_ShouldThrowException(long id)
        {
            // Arrange
            var options = GetInMemoryOptions();
            var mapper = GetMapper();

            ClearDataBase(options);

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);

                // Act
                // Assert
                await Assert.ThrowsAsync<System.ArgumentException>("id", () => service.DeleteTodoItemAsync(id));
            }

            ClearDataBase(options);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(2)]
        public async Task DeleteTodoItemAsync_WithInvalidIdAndExistingElements_ShouldThrowException(long id)
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

                // Assert
                await Assert.ThrowsAsync<System.ArgumentException>("id", () => service.DeleteTodoItemAsync(id));
            }

            ClearDataBase(options);
        }

        [Theory]
        [InlineData(1)] // Delete first
        [InlineData(2)] // Delete middle element
        [InlineData(3)] // Delete last
        public async Task DeleteTodoItemAsync_WithValidIdAndExistingElements_ShouldDeleteIt(long id)
        {
            // Arrange
            var todoItems = new[] {
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

                foreach (var todoItem in todoItems)
                {
                    tasks.Add(context.TodoItems.AddAsync(todoItem));
                }

                await Task.WhenAll(tasks);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);

                // Assert
                var todoItemDTO = await service.DeleteTodoItemAsync(id);
                Assert.NotNull(todoItemDTO);

                // Como el DTO no devuelve el id, verifico que ya no exista en la base
                foreach (var todoItem in context.TodoItems.AsEnumerable()) {
                    Assert.NotEqual(id, todoItem.Id);
                }
            }

            ClearDataBase(options);
        }
    }
}