using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Xunit;

namespace TodoApi.IntegrationTests.Controller {
    public static class TodoControllerShould {
        public static async Task GetTodoItems_WithNoToken_ShouldGetUnauthorizedError(HttpClient client) {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/todo");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public static async Task GetTodoItem_WithNoToken_ShouldGetUnauthorizedError(HttpClient client, int id) {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/todo/" + id);

            // Act:
            var response = await client.SendAsync(request);

            // Assert:
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public static async Task PostTodoItem_WithNoToken_ShouldGetUnauthorizedError(HttpClient client) {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/todo");
            var parameters = new Dictionary<string, string> {
                { "Name", "TodoItem 1" },
                { "IsComplete", "false" },
                { "Order", "0" },
                { "Description", "TodoItem 1 description" }
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public static async Task PutTodoItem_WithNoToken_ShouldGetUnauthorizedError(HttpClient client, int id) {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Put, "/api/todo/" + id);
            var parameters = new Dictionary<string, string> {
                { "Name", "TodoItem 1" },
                { "IsComplete", "false" },
                { "Order", "0" },
                { "Description", "TodoItem 1 description" }
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public static async Task PatchTodoItem_WithNoToken_ShouldGetUnauthorizedError(HttpClient client, int id) {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Patch, "/api/todo/" + id);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public static async Task DeleteTodoItem_WithNoToken_ShouldGetUnauthorizedError(HttpClient client, int id) {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/todo/" + id);

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}