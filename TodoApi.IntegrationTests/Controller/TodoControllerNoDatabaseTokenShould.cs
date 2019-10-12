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
    public class TodoControllerNoDatabaseTokenShould : IClassFixture<SimpleTestFixture> {
        private readonly HttpClient _client;

        public TodoControllerNoDatabaseTokenShould(SimpleTestFixture fixture) {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GetTodoItems_WithTokenAndEmptyDb_ShouldReturnElements() {
            
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetTodoItem_WithTokenAndEmptyDb_ShouldGetNotFoundError(int id) {

        }

        [Fact]
        public async Task PostTodoItem_WithTokenAndEmptyDb_ShouldInsertItem() {

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task PutTodoItem_WithTokenAndEmptyDb_ShouldGetNotFoundError(int id) {

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task PatchTodoItem_WithTokenAndEmptyDb_ShouldGetNotFoundError(int id) {

        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
       public async Task DeleteTodoItem_WithTokenAndEmptyDb_ShouldGetNotFoundError(int id) {

       }
    }
}