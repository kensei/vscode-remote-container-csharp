using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApi.Models;

namespace TodoApi.Tests.TodoApi
{
    public class TodoApiWebApplicationFactory: WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TodoContext>));
                services.Remove(descriptor);

                var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
                services.AddDbContext<TodoContext>(
                    options => options
                        .UseMySql("server=mysql;user=test;password=secret;Database=test", serverVersion)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                    );

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var dbContext = scopedServices.GetRequiredService<TodoContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<TodoApiWebApplicationFactory>>();
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();

                    try
                    {
                        InitializeDbForTests(dbContext);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        public static void InitializeDbForTests(TodoContext db)
        {
            db.TodoItems.AddRange(GetSeedingMessages());
            db.SaveChanges();
        }

        public static List<TodoItem> GetSeedingMessages()
        {
            return new List<TodoItem>()
            {
                new TodoItem() { Id = 1, Name = "hoge", IsComplete = false },
                new TodoItem() { Id = 2, Name = "fuga", IsComplete = true },
                new TodoItem() { Id = 3, Name = "piyo", IsComplete = false },
            };
        }
    }
}