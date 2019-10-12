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
using TodoApi.IntegrationTests.Utilities;
using Xunit;

namespace TodoApi.IntegrationTests.Controller {
    public class TodoControllerDatabaseNoTokenShould : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public TodoControllerDatabaseNoTokenShould(CustomWebApplicationFactory<Startup> factory) {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetTodoItems_WithNoTokenAndElements_ShouldGetUnauthorizedError() {
            await TodoControllerShould.GetTodoItems_WithNoToken_ShouldGetUnauthorizedError(_client);
        }

        [Theory]
        [InlineData(-1)] // Should return Unauthorized instead of Not Found in empty database
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetTodoItem_WithNoTokenAndElements_ShouldGetUnauthorizedError(int id) {
            await TodoControllerShould.GetTodoItem_WithNoToken_ShouldGetUnauthorizedError(_client, id);
        }

        [Fact]
        public async Task PostTodoItem_WithNoTokenAndElements_ShouldGetUnauthorizedError() {
            await TodoControllerShould.PostTodoItem_WithNoToken_ShouldGetUnauthorizedError(_client);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task PutTodoItem_WithNoTokenAndElements_ShouldGetUnauthorizedError(int id) {
            await TodoControllerShould.PutTodoItem_WithNoToken_ShouldGetUnauthorizedError(_client, id);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(2)]
        public async Task PutTodoItem_WithTokenAndElementsAndInvalidId_ShouldGetNotFoundError(int id) {

        }

        [Fact]
        public async Task PutTodoItem_WithTokenAndElementsAndValidId_ShouldPutItem() {

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task PatchTodoItem_WithNoTokenAndElements_ShouldGetUnauthorizedError(int id) {
            await TodoControllerShould.PatchTodoItem_WithNoToken_ShouldGetUnauthorizedError(_client, id);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(2)]
        public async Task PatchTodoItem_WithTokenAndElementsAndInvalidId_ShouldGetNotFoundError(int id) {

        }

        [Fact]
        public async Task PatchTodoItem_WithTokenAndElementsAndValidId_ShouldPatchItem() {

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task DeleteTodoItem_WithNoTokenAndElements_ShouldGetUnauthorizedError(int id) {
            await TodoControllerShould.DeleteTodoItem_WithNoToken_ShouldGetUnauthorizedError(_client, id);
       }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(2)]
        public async Task DeleteTodoItem_WithTokenAndElementsAndInvalidId_ShouldGetNotFoundError(int id) {

        }

        [Fact]
        public async Task DeleteTodoItem_WithTokenAndElementsAndValidId_ShouldDeleteItem() {

        }

        public void Dispose() {
            _client.Dispose();
        }
    }
}