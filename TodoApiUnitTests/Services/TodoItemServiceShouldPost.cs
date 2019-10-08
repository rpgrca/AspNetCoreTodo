using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using TodoApi.Models;
using TodoApi.DTO;
using TodoApi.Services;

namespace TodoApi.UnitTests.Services
{
    public partial class TodoItemServiceShould
    {
        [Fact]
        public async Task PostTodoItemAsync_WithEmptyList_ShouldAddIt()
        {
            // Arrange
            var options = GetInMemoryOptions();
            var mapper = GetMapper();
            var todoItemDTO = mapper.Map<TodoItemDTO>(CreateFakeTodoItem());

            ClearDataBase(options);

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);

                // Act
                await service.PostTodoItemAsync(todoItemDTO);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Assert
                Assert.True(context.TodoItems.Count() == 1);
                var todoItem = await context.TodoItems.FirstAsync();
                Assert.NotNull(todoItem);

                Assert.Equal(todoItemDTO.Description, todoItem.Description);
                Assert.Equal(todoItemDTO.IsComplete, todoItem.IsComplete);
                Assert.Equal(todoItemDTO.Name, todoItem.Name);
                Assert.Equal(todoItemDTO.DueAt, todoItem.DueAt);
                Assert.Equal(todoItemDTO.Order, todoItem.Order);
            }

            ClearDataBase(options);
        }

        [Fact]
        public async Task PostTodoItemAsync_WithExistingElements_ShouldAddIt()
        {
            // Arrange
            var options = GetInMemoryOptions();
            var mapper = GetMapper();
            var existingItem = CreateFakeTodoItem(0);
            var todoItemDTO = mapper.Map<TodoItemDTO>(CreateFakeTodoItem(2));

            ClearDataBase(options);

            using (var context = new ApplicationDbContext(options))
            {
                await context.TodoItems.AddAsync(existingItem);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);

                // Act
                await service.PostTodoItemAsync(todoItemDTO);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Assert
                Assert.True(context.TodoItems.Count() == 2);
                var todoItem = await context.TodoItems.LastAsync();
                Assert.NotNull(todoItem);

                Assert.Equal(todoItemDTO.Description, todoItem.Description);
                Assert.Equal(todoItemDTO.IsComplete, todoItem.IsComplete);
                Assert.Equal(todoItemDTO.Name, todoItem.Name);
                Assert.Equal(todoItemDTO.DueAt, todoItem.DueAt);
                Assert.Equal(todoItemDTO.Order, todoItem.Order);
            }

            ClearDataBase(options);
        }
    }
}