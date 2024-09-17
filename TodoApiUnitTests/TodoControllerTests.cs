using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApiUnitTests;

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
    public void GetAll_ReturnsListWithCorrectNumberOfItems()
    {
        // Act (synchronous call of controller action)
        var result = controller.GetAll().Result as List<TodoItem>;
        var noOfItems = result.Count;

        // Assert
        Assert.Equal(2, noOfItems);
    }

    [Fact]
    public async Task GetAll_ReturnsListWithCorrectNumberOfItems_Async()
    {
        // Act (asynchronous call of controller action)
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