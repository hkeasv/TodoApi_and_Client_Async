using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApiUnitTests
{
    public class TodoControllerTests
    {
        private readonly Mock<IRepository<TodoItem>> _mockRepo;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockRepo = new Mock<IRepository<TodoItem>>();
            _controller = new TodoController(_mockRepo.Object);
        }
        
        [Fact]
        public void GetAll_ReturnsAllItems_WhenInvokedSync()
        {
            // Arrange
            var items = new List<TodoItem> { new TodoItem { Id = 1, Name = "Test Item 1" } };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);

            // Act
            var result = _controller.GetAll().Result as List<TodoItem>;

            // Assert
            Assert.Equal(items, result);
        }

        [Fact]
        public async Task GetAll_ReturnsAllItems_WhenInvokedAsync()
        {
            // Arrange
            var items = new List<TodoItem> { new TodoItem { Id = 1, Name = "Test Item 1" } };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.Equal(items, result);
        }

        [Fact]
        public async Task Get_ReturnsItem_WhenItemExists()
        {
            // Arrange
            var item = new TodoItem { Id = 1, Name = "Test Item" };
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync(item);

            // Act
            var result = await _controller.Get(1) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(item, result.Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtAction_WhenItemIsValid()
        {
            // Arrange
            var item = new TodoItem { Id = 1, Name = "Test Item" };

            // Act
            var result = await _controller.Post(item) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(item, result.Value);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenItemIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsNoContent_WhenItemIsUpdated()
        {
            // Arrange
            var item = new TodoItem { Id = 1, Name = "Updated Item" };
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync(item);

            // Act
            var result = await _controller.Put(1, item);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenItemIsNull()
        {
            // Act
            var result = await _controller.Put(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenItemIsDeleted()
        {
            // Arrange
            var item = new TodoItem { Id = 1, Name = "Test Item" };
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync(item);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}