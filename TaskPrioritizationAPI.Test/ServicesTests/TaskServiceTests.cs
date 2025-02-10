using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TaskPrioritizationAPI.Core.Services;
using TaskPrioritizationAPI.Core.Models;
using TaskPrioritizationAPI.Core.Constants;
using TaskPrioritizationAPI.Infrastructure.Data;
using TaskPrioritizationAPI.Infrastructure.Data.Common;
using TaskPrioritizationAPI.Infrastructure.Data.Repositories;

namespace TaskPrioritizationAPI.Test.ServiceTests
{
    public class TaskServiceTests
    {
        private readonly TaskService _taskService;
        private readonly Mock<IApplicationDbRepository> _mockRepo;

        public TaskServiceTests()
        {
            _mockRepo = new Mock<IApplicationDbRepository>();
            _taskService = new TaskService(_mockRepo.Object);
        }

        // 1️ Test for AddTask
        [Fact]
        public async System.Threading.Tasks.Task AddTask_ShouldReturnTaskViewModel_WhenTaskIsAdded()
        {
            // Arrange
            var taskAddModel = new TaskAddViewModel
            {
                Title = "New Task",
                Description = "Test Task",
                DueDate = "2025-02-15",
                IsCritical = true
            };

            var task = new TaskPrioritizationAPI.Infrastructure.Data.Task
            {
                Title = taskAddModel.Title,
                Description = taskAddModel.Description,
                DueDate = taskAddModel.DueDate,
                IsCritical = taskAddModel.IsCritical,
                Priority = PriorityLevel.High
            };

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<TaskPrioritizationAPI.Infrastructure.Data.Task>()))
                     .Returns(System.Threading.Tasks.Task.CompletedTask);

            _mockRepo.Setup(repo => repo.SaveChangesAsync())
                     .ReturnsAsync(1);

            // Act
            var result = await _taskService.AddTask(taskAddModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskAddModel.Title, result.Title);
            Assert.Equal(taskAddModel.Description, result.Description);
            Assert.Equal(PriorityLevel.High, result.Priority);
        }

        // 2️ Test for GetAllTasks
        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<TaskPrioritizationAPI.Infrastructure.Data.Task>
        {
            new TaskPrioritizationAPI.Infrastructure.Data.Task
            {
                Title = "Task 1",
                Description = "Description 1",
                DueDate = "2025-02-15",
                IsCompleted = false
            },
            new TaskPrioritizationAPI.Infrastructure.Data.Task
            {
                Title = "Task 2",
                Description = "Description 2",
                DueDate = "2025-02-20",
                IsCompleted = true
            }
        };

            _mockRepo.Setup(repo => repo.All<TaskPrioritizationAPI.Infrastructure.Data.Task>())
                     .Returns(tasks.AsQueryable());

            // Act
            var result = await _taskService.GetAllTasks();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        // 3️ Test for GetTaskById
        [Fact]
        public async System.Threading.Tasks.Task GetTaskById_ShouldReturnTask_WhenExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskPrioritizationAPI.Infrastructure.Data.Task
            {
                Id = taskId,
                Title = "Existing Task",
                Description = "Test Desc",
                DueDate = "2025-02-10",
                IsCompleted = false
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync<TaskPrioritizationAPI.Infrastructure.Data.Task>(taskId))
                     .ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskById(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
        }

        // 4️ Test for RemoveTaskById
        [Fact]
        public async System.Threading.Tasks.Task RemoveTaskById_ShouldReturnTrue_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskPrioritizationAPI.Infrastructure.Data.Task { Id = taskId };

            _mockRepo.Setup(repo => repo.GetByIdAsync<TaskPrioritizationAPI.Infrastructure.Data.Task>(taskId))
                     .ReturnsAsync(task);

            _mockRepo.Setup(repo => repo.DeleteAsync<TaskPrioritizationAPI.Infrastructure.Data.Task>(taskId))
                     .Returns(System.Threading.Tasks.Task.CompletedTask);

            _mockRepo.Setup(repo => repo.SaveChangesAsync())
                     .ReturnsAsync(1);

            // Act
            var result = await _taskService.RemoveTaskById(taskId);

            // Assert
            Assert.True(result);
        }

        // 5️ Test for RemoveTaskById When Task Doesn't Exist
        [Fact]
        public async System.Threading.Tasks.Task RemoveTaskById_ShouldReturnFalse_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _mockRepo.Setup(repo => repo.GetByIdAsync<TaskPrioritizationAPI.Infrastructure.Data.Task>(taskId))
                     .ReturnsAsync((TaskPrioritizationAPI.Infrastructure.Data.Task)null);

            // Act
            var result = await _taskService.RemoveTaskById(taskId);

            // Assert
            Assert.False(result);
        }

        // 6️ Test for UpgateTask
        [Fact]
        public async System.Threading.Tasks.Task UpdateTask_ShouldReturnTrue_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var existingTask = new TaskPrioritizationAPI.Infrastructure.Data.Task
            {
                Id = taskId,
                Description = "Old Desc",
                DueDate = "2025-02-15",
                IsCompleted = false
            };

            var updatedTaskViewModel = new TaskViewModel
            {
                Id = taskId,
                Description = "New Desc",
                DueDate = "2025-02-20",
                IsCompleted = true
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync<TaskPrioritizationAPI.Infrastructure.Data.Task>(taskId))
                     .ReturnsAsync(existingTask);

            _mockRepo.Setup(repo => repo.SaveChangesAsync())
                     .ReturnsAsync(1);

            // Act
            var result = await _taskService.UpgateTask(updatedTaskViewModel);

            // Assert
            Assert.True(result);
            Assert.Equal("New Desc", existingTask.Description);
        }

        // 7️ Test for OrderTasksByPriority
        [Fact]
        public async System.Threading.Tasks.Task OrderTasksByPriority_ShouldReturnTasksOrderedByPriority()
        {
            // Arrange
            var tasks = new List<TaskViewModel>
        {
            new TaskViewModel { Title = "Task 1", Priority = PriorityLevel.Medium , DueDate = "2025-02-20" , IsCompleted = false , IsCritical = false },
            new TaskViewModel { Title = "Task 2", Priority = PriorityLevel.Medium , DueDate = "2025-02-20" , IsCompleted = false , IsCritical = true },
            new TaskViewModel { Title = "Task 3", Priority = PriorityLevel.High , DueDate = "2025-02-20" , IsCompleted = true , IsCritical = false }
        };

            // Act
            var result = await _taskService.OrderTasksByPriority(tasks);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(PriorityLevel.High, result.First().Priority);
        }
    }
}
    
