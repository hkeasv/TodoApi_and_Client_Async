using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApiIntegrationTests;

public class TodoControllerTests : IDisposable
{
    SqliteConnection connection;
    TodoController controller;
    TodoContext dbContext;

    public TodoControllerTests()
    {
        connection = new SqliteConnection("Data Source=TodoTestDb.db");
        connection.Open();
        var options = new DbContextOptionsBuilder<TodoContext>().UseSqlite(connection).Options;
        dbContext = new TodoContext(options);
        dbContext.Database.EnsureCreated();
        if (!dbContext.TodoItems.Any())
        {
            dbContext.TodoItems.Add(new TodoItem { Name = "Item1" });
            dbContext.TodoItems.Add(new TodoItem { Name = "Item2" });
            dbContext.SaveChanges();
        }

        controller = new TodoController(new TodoItemRepository(dbContext));
    }
    
    public void Dispose()
    {
        connection.Close();
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
    
    [Fact]
    public async Task Post_ReturnsCreatedAtAction_WhenItemIsValid()
    {
        // Arrange
        var item = new TodoItem { Name = "Test Item" };
        // Start a transaction to ensure the database state is rolled back after the test
        var transaction = await dbContext.Database.BeginTransactionAsync();


        // Act
        var result = await controller.Post(item) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal(item, result.Value);
        
        // Rollback the transaction to clean up the database
        await transaction.RollbackAsync();
    }
    
}