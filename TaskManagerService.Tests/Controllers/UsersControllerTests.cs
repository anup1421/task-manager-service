using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using TaskManagerService.Api.Controllers;
using TaskManagerService.Core.Interfaces;
using TaskManagerService.Core.Models;
using Xunit;

namespace TaskManagerService.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnOkResult()
        {
            // Arrange
            var userDto = new UserDto
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "Test",
                LastName = "User"
            };

            var expectedUser = new User
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                CreatedAt = DateTime.UtcNow
            };

            _mockUserService.Setup(x => x.RegisterAsync(It.IsAny<UserDto>()))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OkObjectResult>(okResult).Value;
            Assert.Contains("User registered successfully", response.ToString());
        }

        [Fact]
        public async Task LoginUser_ShouldReturnOkResult()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var token = new TokenResponse
            {
                Token = "test-token",
                RefreshToken = "test-refresh-token",
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _mockUserService.Setup(x => x.LoginAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync(token);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<TokenResponse>(okResult.Value);
            Assert.Equal(token.Token, response.Token);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnOkResult()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                Id = userId,
                Email = "test@example.com"
            };

            _mockUserService.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<User>(okResult.Value);
            Assert.Equal(userId, response.Id);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOkResult()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Email = "user1@example.com" },
                new User { Id = 2, Email = "user2@example.com" }
            };

            _mockUserService.Setup(x => x.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<IEnumerable<User>>(okResult.Value);
            Assert.Equal(2, response.Count());
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnOkResult()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDto
            {
                Email = "updated@example.com",
                FirstName = "Updated",
                LastName = "User"
            };

            var updatedUser = new User
            {
                Id = userId,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };

            _mockUserService.Setup(x => x.UpdateUserAsync(userId, It.IsAny<UserDto>()))
                .ReturnsAsync(updatedUser);

            // Act
            var result = await _controller.UpdateUser(userId, userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<User>(okResult.Value);
            Assert.Equal(updatedUser.Email, response.Email);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockUserService.Verify(x => x.DeleteUserAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound()
        {
            // Arrange
            var userId = 1;
            _mockUserService.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
