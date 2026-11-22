using Api.Controllers;
using Ddd.App.Commands.Config;
using Ddd.App.Db;
using Ddd.App.Entities;
using Ddd.App.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ddd.App.Api.UnitTests.Controllers
{
    public class ConfigControllerTests
    {
        private readonly ConfigsController _controller;
        private readonly Mock<IConfigService> _service;
        public ConfigControllerTests()
        {
            _service = new Mock<IConfigService>();
            _controller = new ConfigsController(_service.Object);
        }
        [Fact]
        public async Task PostAsync_OkObjectResult()
        {
            // Arrange
            var cmd = new ConfigCreateCmd()
            {
                Name = "TestConfig"
            };
            var config = new Config();
            config.Rename(new ConfigRenameCmd() { NewName = cmd.Name });
            config.ClearEvents();
            _service.Setup(s => s.CreateAsync(cmd)).ReturnsAsync(config);
            // Act
            var result = await _controller.PostAsync(cmd);
            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Config? returnedConfig = okObjectResult.Value as Config;
            Assert.Equal(config, returnedConfig);
        }
        [Fact]
        public async Task PostAsync_ConflictResult()
        {
            // Arrange
            var cmd = new ConfigCreateCmd()
            {
                Name = "TestConfig"
            };
            var existingConfig = new Config();
            existingConfig.Rename(new ConfigRenameCmd() { NewName = "TestConfig2" });
            existingConfig.ClearEvents();
            _service.Setup(s => s.CreateAsync(cmd)).ThrowsAsync(
                new DbEntityMultipleSingletonsException<Config>(existingConfig));
            // Act
            var result = await _controller.PostAsync(cmd);
            // Assert
            var conflictResult = result as ConflictResult;
            Assert.NotNull(conflictResult);
        }
        [Fact]
        public async Task GetAsync_OkObjectResult()
        {
            // Arrange
            var config = new Config();
            config.Rename(new ConfigRenameCmd() { NewName = "TestConfig" });
            config.ClearEvents();
            _service.Setup(s => s.GetAsync()).ReturnsAsync(config);
            // Act
            var result = await _controller.GetAsync();
            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Config? returnedConfig = okObjectResult.Value as Config;
            Assert.Equal(config, returnedConfig);
        }
        [Fact]
        public async Task GetAsync_NotFoundResult()
        {
            _service.Setup(s => s.GetAsync()).ThrowsAsync(new DbSingletonEntityNotFoundException(typeof(Config)));
            // Act
            var result = await _controller.GetAsync();
            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }
        [Fact]
        public async Task NameAsync_OkObjectResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cmd = new ConfigRenameCmd()
            {
                NewName = "NewConfigName"
            };
            _service.Setup(s => s.RenameAsync(id, cmd)).ThrowsAsync(new DbSingletonEntityNotFoundException(typeof(Config)));
            // Act
            var result = await _controller.NameAsync(id, cmd);
            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }
        [Fact]
        public async Task DeleteAsync_OkResult()
        {
            // Arrange
            _service.Setup(s => s.DeleteAsync()).Returns(Task.CompletedTask);
            // Act
            var result = await _controller.DeleteAsync();
            // Assert
            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
        }
        //DbSingletonEntityNotFoundException
        [Fact]
        public async Task DeleteAsync_NotFoundResult()
        {
            // Arrange
            _service.Setup(s => s.DeleteAsync()).ThrowsAsync(new DbSingletonEntityNotFoundException(typeof(Config)));
            // Act
            var result = await _controller.DeleteAsync();
            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.NotNull(notFoundResult);
        }
    }
}
