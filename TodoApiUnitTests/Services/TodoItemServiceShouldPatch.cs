using Xunit;
using System.Threading.Tasks;
using System.Linq;
using TodoApi.Models;
using TodoApi.DTO;
using TodoApi.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace TodoApi.UnitTests.Services
{
    public partial class TodoItemServiceShould
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task PatchTodoItemAsync_WithInvalidIdAndEmptyList_ShouldThrowException(long id)
        {
            // Arrange
            var options = GetInMemoryOptions();
            var mapper = GetMapper();
            var patch = new JsonPatchDocument<TodoItemDTO>(); // todoItemDTO = mapper.Map<TodoItemDTO>(CreateFakeTodoItem());

            ClearDataBase(options);

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context, mapper);

                // Assert
                await Assert.ThrowsAsync<ArgumentException>("id", async () => await service.PatchTodoItemAsync(id, patch));
            }

            ClearDataBase(options);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(2)]
        public async Task PatchTodoItemAsync_WithInvalidIdAndExistingElements_ShouldThrowException(long id)
        {
            // Arrange
            var options = GetInMemoryOptions();
            var mapper = GetMapper();
            var existingItem = CreateFakeTodoItem(1);
            var patch = new JsonPatchDocument<TodoItemDTO>();

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
                // Assert
                await Assert.ThrowsAsync<ArgumentException>("id", async () => await service.PatchTodoItemAsync(id, patch));
            }

            ClearDataBase(options);
        }

        [Theory]
        [InlineData(1, "TODO ITEM 1", "[{ \"op\": \"replace\", \"path\": \"Name\", \"value\": \"TODO ITEM 1\" }]")] // First element
        [InlineData(2, "TODO ITEM 2", "[{ \"op\": \"replace\", \"path\": \"Name\", \"value\": \"TODO ITEM 2\" }]")] // Middle
        [InlineData(3, "TODO ITEM 3", "[{ \"op\": \"replace\", \"path\": \"Name\", \"value\": \"TODO ITEM 3\" }]")] // Last element
        public async Task PatchTodoItemAsync_WithValidIdAndExistingElement_ShouldUpdateItem(long id, string newName, string jsonPatch)
        {
            // Arrange
            var expectedTodoItems = new[] {
                CreateFakeTodoItem(1),
                CreateFakeTodoItem(2),
                CreateFakeTodoItem(3)
            };
            var options = GetInMemoryOptions();
            var mapper = GetMapper();
            var updatedTodoItem = mapper.Map<TodoItemDTO>(CreateFakeTodoItem(id));
            updatedTodoItem.Name = newName;

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
            TodoItem modifiedItem = null;

            using (var context = new ApplicationDbContext(options))
            {
                var jsonPatchDocument = JsonConvert.DeserializeObject<JsonPatchDocument<TodoItemDTO>>(jsonPatch);

                var service = new TodoItemService(context, mapper);
                modifiedItem = await service.PatchTodoItemAsync(id, jsonPatchDocument);
                await context.SaveChangesAsync();
            }

            // Assert
            Assert.NotNull(modifiedItem);
            Assert.Equal(updatedTodoItem.Name, modifiedItem.Name);
            Assert.Equal(updatedTodoItem.Description, modifiedItem.Description);
            Assert.Equal(updatedTodoItem.IsComplete, modifiedItem.IsComplete);
            Assert.Equal(updatedTodoItem.DueAt, modifiedItem.DueAt);
            Assert.Equal(updatedTodoItem.Order, modifiedItem.Order);

            using (var context = new ApplicationDbContext(options))
            {
                Assert.True(context.TodoItems.Count() == 3);

                var todoItem = await context.TodoItems.FindAsync(id);

                Assert.NotNull(todoItem);
                Assert.Equal(updatedTodoItem.Name, todoItem.Name);
                Assert.Equal(updatedTodoItem.Description, todoItem.Description);
                Assert.Equal(updatedTodoItem.IsComplete, todoItem.IsComplete);
                Assert.Equal(updatedTodoItem.DueAt, todoItem.DueAt);
                Assert.Equal(updatedTodoItem.Order, todoItem.Order);
            }

            ClearDataBase(options);
        }
    }
}