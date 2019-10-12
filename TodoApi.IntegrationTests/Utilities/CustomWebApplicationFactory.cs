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

namespace TodoApi.IntegrationTests.Utilities {
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class {
            protected override void ConfigureWebHost(IWebHostBuilder builder) {
                builder.ConfigureServices(services => {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<ApplicationDbContext>((options, context) => {
                        context.UseInMemoryDatabase("InMemoryDbForTesting")
                            .UseInternalServiceProvider(serviceProvider);
                    });

                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope()) {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                        var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                        db.Database.EnsureCreated();

                        try {
                            ClearDataBase(db);
                            InitializeDbForTests(db);
                        }
                        catch (Exception ex) {
                            logger.LogError(ex, "An error occurred seeding the database with test data. Error: {Message}", ex.Message);
                        }
                    }
                });
            }

            private static void ClearDataBase(ApplicationDbContext db) {
                db.Database.EnsureDeleted();
            }

            private static void InitializeDbForTests(ApplicationDbContext db) {
                db.TodoItems.Add(new TodoApi.Models.TodoItem {
                    Id = 1,
                    Name = "TodoItem 1",
                    IsComplete = false,
                    Description = "Description 1",
                    Order = 0
                });
            }
        }
}