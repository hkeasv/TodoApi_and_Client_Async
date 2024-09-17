using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApiIntegrationTests;

public class TodoControllerTests
{
    SqliteConnection connection;
    TodoController controller;

    public TodoControllerTests()
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<TodoContext>().UseSqlite(connection).Options;
        var dbContext = new TodoContext(options);
        dbContext.Database.EnsureCreated();
        dbContext.TodoItems.Add(new TodoItem { Name = "Item1" });
        dbContext.TodoItems.Add(new TodoItem { Name = "Item2" });
        dbContext.SaveChanges();
        controller = new TodoController(new TodoItemRepository(dbContext));
    }
    
    [Fact]
    public async Task GetAll_ReturnsListWithCorrectNumberOfItems_Async()
    {
        // Act
        var result = await controller.GetAll();
        var items = result as List<TodoItem>;
        var noOfItems = items.Count;

        // Assert
        Assert.Equal(2, noOfItems);
    }

    public void Dispose()
    {
        connection.Close();
    }

}