using Xunit;
using System.Threading.Tasks;
using System.Linq;
using TodoApi.Models;
using TodoApi.DTO;
using TodoApi.Services;
using System;
using System.Collections.Generic;

namespace TodoApi.UnitTests.Services
{
    public partial class TodoItemServiceShould
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("TodoItem")]
        public async Task SearchTodoItemsAsync_WithWhateverTextAndEmptyList_ShouldReturnEmpty(string text)
        {
            // Arrange
            var options = GetInMemoryOptions();
            var mapper = GetMapper();

            ClearDataBase(options);

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);
                var todoItems = await service.SearchTodoItemsAsync(text);

                // Assert
                Assert.NotNull(todoItems);
                Assert.Empty(todoItems);
            }

            ClearDataBase(options);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task SearchTodoItemsAsync_WithEmptyOrNullAndExistingItems_ShouldReturnEverything(string text)
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
                var todoItemDTOs = await service.SearchTodoItemsAsync(text);

                // Assert
                Assert.NotNull(todoItemDTOs);
                Assert.True(todoItemDTOs.Count == 3);

                for (int index = 0; index < todoItemDTOs.Count; index++)
                {
                    Assert.Equal(expectedTodoItems[index].Name, todoItemDTOs[index].Name);
                    Assert.Equal(expectedTodoItems[index].Description, todoItemDTOs[index].Description);
                    Assert.Equal(expectedTodoItems[index].DueAt, todoItemDTOs[index].DueAt);
                    Assert.Equal(expectedTodoItems[index].IsComplete, todoItemDTOs[index].IsComplete);
                    Assert.Equal(expectedTodoItems[index].Order, todoItemDTOs[index].Order);
                }
            }

            ClearDataBase(options);
        }

        [Theory]
        [InlineData("TodoItem 1", 1)]
        [InlineData("TODOITEM 1", 1)] // Ignorecase
        [InlineData("TodoItem", 3)]
        public async Task SearchTodoItemAsync_WithCorrectTextAndExistingItems_ShouldReturnSomething(string text, int amount)
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

                foreach (var expectedTodoItem in expectedTodoItems)
                {
                    tasks.Add(context.TodoItems.AddAsync(expectedTodoItem));
                }

                await Task.WhenAll(tasks);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);
                var todoItemDTOs = await service.SearchTodoItemsAsync(text);

                // Assert
                Assert.NotNull(todoItemDTOs);
                Assert.True(todoItemDTOs.Count == amount);

                for (int index = 0; index < amount; index++)
                {
                    Assert.Equal(expectedTodoItems[index].Name, todoItemDTOs[index].Name);
                    Assert.Equal(expectedTodoItems[index].Description, todoItemDTOs[index].Description);
                    Assert.Equal(expectedTodoItems[index].DueAt, todoItemDTOs[index].DueAt);
                    Assert.Equal(expectedTodoItems[index].IsComplete, todoItemDTOs[index].IsComplete);
                    Assert.Equal(expectedTodoItems[index].Order, todoItemDTOs[index].Order);
                }
            }

            ClearDataBase(options);
        }
    }
}