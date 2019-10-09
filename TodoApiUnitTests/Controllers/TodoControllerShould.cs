using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.DTO;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApi.UnitTests.Services
{
    public class TodoControllerShould {
        private const long _DefaultItemDTOId = 1;
        private const string _DefaultItemDTOName = "TodoItemDTO";
        private const string _DefaultItemDTODescription = "Esta es la descripcion del TodoItemDTO";
        private const long _DefaultItemDTOOrder = 0;

        [Fact]
        public async Task GetTodoItem_WithValidId_ShouldReturnOkViewWithTodoItem() {
            // Arrange
            var expectedTodoItemDTO = CreateFakeTodoItemDTO();
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetTodoItemAsync(1))
                       .ReturnsAsync(expectedTodoItemDTO);

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.GetTodoItem(1);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItemDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            var todoItemDTO = Assert.IsAssignableFrom<TodoItemDTO>(okResult.Value);

            Assert.Equal(expectedTodoItemDTO.Name, todoItemDTO.Name);
            Assert.Equal(expectedTodoItemDTO.Description, todoItemDTO.Description);
            Assert.Equal(expectedTodoItemDTO.DueAt, todoItemDTO.DueAt);
            Assert.Equal(expectedTodoItemDTO.IsComplete, todoItemDTO.IsComplete);
            Assert.Equal(expectedTodoItemDTO.Order, todoItemDTO.Order);
        }

        [Fact]
        public async Task GetTodoItem_WithInvalidIdOrEmptyList_ShouldReturnNotFoundView() {
            // Arrange
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetTodoItemAsync(1))
                       .ReturnsAsync((TodoItemDTO)null);

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.GetTodoItem(1);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItemDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetTodoItems_WithExistingElements_ShouldReturnOkWithTodoItems() {
            // Arrange
            var expectedTodoItemDTOs = new List<TodoItemDTO>() { CreateFakeTodoItemDTO(1), CreateFakeTodoItemDTO(2) };
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetTodoItemsAsync())
                       .ReturnsAsync((expectedTodoItemDTOs));

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.GetTodoItems();

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            var todoItemsDTO = Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(okResult.Value);
            Assert.Equal(expectedTodoItemDTOs.Count, todoItemsDTO.Count());

            var todoItemsDTOList = todoItemsDTO.ToList();
            for (int index = 0; index < expectedTodoItemDTOs.Count; index++) {
                Assert.Equal(expectedTodoItemDTOs[index].Name, todoItemsDTOList[index].Name);
                Assert.Equal(expectedTodoItemDTOs[index].Description, todoItemsDTOList[index].Description);
                Assert.Equal(expectedTodoItemDTOs[index].DueAt, todoItemsDTOList[index].DueAt);
                Assert.Equal(expectedTodoItemDTOs[index].IsComplete, todoItemsDTOList[index].IsComplete);
                Assert.Equal(expectedTodoItemDTOs[index].Order, todoItemsDTOList[index].Order);
            }
        }

        [Fact]
        public async Task GetTodoItems_WithEmptyList_ShouldReturnEmptyList() {
            // Arrange
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.GetTodoItemsAsync())
                       .ReturnsAsync(new List<TodoItemDTO>());

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.GetTodoItems();

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            var todoItemsDTO = Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(okResult.Value);
            Assert.True(0 == todoItemsDTO.Count());
        }

        [Fact]
        public async Task PostTodoItem_WithNewItem_ShouldReturnCreatedAtAction() {
            // Arrange
            var todoItemDTO = CreateFakeTodoItemDTO();
            var expectedTodoItem = new TodoItem {
                Id = 1
            };

            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.PostTodoItemAsync(todoItemDTO))
                       .ReturnsAsync(expectedTodoItem);

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.PostTodoItem(todoItemDTO);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);
            var todoItem = Assert.IsAssignableFrom<TodoItem>(createdResult.Value);
            Assert.Equal(expectedTodoItem.Id, todoItem.Id);
        }

        [Fact]
        public async Task PutTodoItem_WithInvalidId_ShouldReturnNotFound() {
            // Arrange
            var todoItemDTO = CreateFakeTodoItemDTO(1);
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.PutTodoItemAsync(1, todoItemDTO))
                       .ThrowsAsync(new ArgumentException("Invalid id", "id"));

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.PutTodoItem(1, todoItemDTO);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task PutTodoItem_WithValidId_ShouldReturnNoContent() {
            // Arrange
            var todoItemDTO = CreateFakeTodoItemDTO(1);
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.PutTodoItemAsync(1, todoItemDTO))
                       .ReturnsAsync(new TodoItem());

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.PutTodoItem(1, todoItemDTO);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var noContentResult = Assert.IsType<NoContentResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task PatchTodoItem_WithInvalidId_ShouldReturnNotFound() {
            // Arrange
            var mockService = new Mock<ITodoItemService>();
            var patch = new JsonPatchDocument<TodoItemDTO>();
            mockService.Setup(service => service.PatchTodoItemAsync(1, patch))
                       .ThrowsAsync(new ArgumentException("Invalid id", "id"));

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.PatchTodoItem(1, patch);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task PatchTodoItem_WithValidId_ShouldReturnNoContent() {
            // Arrange
            var mockService = new Mock<ITodoItemService>();
            var patch = new JsonPatchDocument<TodoItemDTO>();
            mockService.Setup(service => service.PatchTodoItemAsync(1, patch))
                       .ReturnsAsync(new TodoItem());

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.PatchTodoItem(1, patch);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var noContentResult = Assert.IsType<NoContentResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTodoItem_WithInvalidId_ShouldReturnNotFound() {
            // Arrange
            var todoItemDTO = CreateFakeTodoItemDTO(1);
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.DeleteTodoItemAsync(1))
                       .ThrowsAsync(new ArgumentException("Invalid id", "id"));

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.DeleteTodoItem(1);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItemDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTodoItem_WithValidId_ShouldReturnNoContent() {
            // Arrange
            var expectedTodoItemDTO = CreateFakeTodoItemDTO(1);
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.DeleteTodoItemAsync(1))
                       .ReturnsAsync(expectedTodoItemDTO);

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.DeleteTodoItem(1);

            // Assert
            var viewResult = Assert.IsType<ActionResult<TodoItemDTO>>(result);
            var noContentResult = Assert.IsType<NoContentResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task SearchTodoItems_WithEmptyOrNullText_ShouldReturnEverything(string searchText)
        {
            // Arrange
            var expectedTodoItemDTOs = new List<TodoItemDTO>() { CreateFakeTodoItemDTO(1), CreateFakeTodoItemDTO(2) };
            var mockService = new Mock<ITodoItemService>();
            mockService.Setup(service => service.SearchTodoItemsAsync(searchText))
                       .ReturnsAsync((expectedTodoItemDTOs));

            var controller = new TodoController(mockService.Object);

            // Act
            var result = await controller.SearchTodoItems(searchText);

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(viewResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            var todoItemsDTO = Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(okResult.Value);
            Assert.Equal(expectedTodoItemDTOs.Count, todoItemsDTO.Count());

            var todoItemsDTOList = todoItemsDTO.ToList();
            for (int index = 0; index < expectedTodoItemDTOs.Count; index++)
            {
                Assert.Equal(expectedTodoItemDTOs[index].Name, todoItemsDTOList[index].Name);
                Assert.Equal(expectedTodoItemDTOs[index].Description, todoItemsDTOList[index].Description);
                Assert.Equal(expectedTodoItemDTOs[index].DueAt, todoItemsDTOList[index].DueAt);
                Assert.Equal(expectedTodoItemDTOs[index].IsComplete, todoItemsDTOList[index].IsComplete);
                Assert.Equal(expectedTodoItemDTOs[index].Order, todoItemsDTOList[index].Order);
            }
        }

        private static TodoItemDTO CreateFakeTodoItemDTO(long itemId = _DefaultItemDTOId, string itemName = _DefaultItemDTOName, string itemDescription = _DefaultItemDTODescription, bool completed = false, long order = _DefaultItemDTOOrder, DateTimeOffset dateTimeOffset = default) => new TodoItemDTO
        {
            Name = $"{itemName} {itemId}",
            Description = $"{itemDescription} {itemId}",
            IsComplete = completed,
            Order = order,
            DueAt = dateTimeOffset
        };
    }
}