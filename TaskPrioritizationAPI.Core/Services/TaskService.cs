using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPrioritizationAPI.Core.Constants;
using TaskPrioritizationAPI.Core.Contracts;
using TaskPrioritizationAPI.Core.Models;
using TaskPrioritizationAPI.Infrastructure.Data;
using TaskPrioritizationAPI.Infrastructure.Data.Common;
using TaskPrioritizationAPI.Infrastructure.Data.Repositories;

namespace TaskPrioritizationAPI.Core.Services
{
    public class TaskService : ITaskService
    {
        private readonly IApplicationDbRepository repo;

        public TaskService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<TaskViewModel> AddTask(TaskAddViewModel taskAddModel)
        {
            var task = new Infrastructure.Data.Task()
            {
                Title = taskAddModel.Title,
                Description = taskAddModel.Description,
                Priority = PriorityCalculator(taskAddModel.DueDate, taskAddModel.IsCritical, false),
                DueDate = taskAddModel.DueDate,
                IsCritical = taskAddModel.IsCritical
            };
            
            await repo.AddAsync(task);
            await repo.SaveChangesAsync();

            var taskViewModel = new TaskViewModel()
            {
                Title = taskAddModel.Title,
                Description = taskAddModel.Description,
                Priority = PriorityCalculator(taskAddModel.DueDate, taskAddModel.IsCritical, false),
                DueDate = taskAddModel.DueDate,
                IsCritical = taskAddModel.IsCritical
            };
            return taskViewModel;
        }

        public async Task<IEnumerable<TaskViewModel>> GetAllTasks()
        {
            var tasks = repo.All<Infrastructure.Data.Task>().Select(t => new TaskViewModel()
            {
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted,
                IsCritical = t.IsCritical
            });

            foreach (var task in tasks)
            {
                task.Priority = PriorityCalculator(task.DueDate, task.IsCritical, task.IsCompleted);
            }

            return tasks;

        }

        public async Task<IEnumerable<TaskViewModel>> OrderTasksByPriority(IEnumerable<TaskViewModel> tasks)
        {

            foreach (var task in tasks)
            {
                task.Priority = PriorityCalculator(task.DueDate, task.IsCritical, task.IsCompleted);
            }

            return tasks.OrderByDescending(t => t.Priority);

        }

        public async Task<IEnumerable<TaskViewModel>> OrderTasksByDueDate(IEnumerable<TaskViewModel> tasks)
        {

            foreach (var task in tasks)
            {
                task.Priority = PriorityCalculator(task.DueDate, task.IsCritical, task.IsCompleted);
            }

            return tasks.OrderByDescending((t) => ConvertToDateTime(t.DueDate));

        }

        private static DateTime ConvertToDateTime(string dateString)
        {
            DateTime.TryParse(dateString, out DateTime parsedDate);
            return parsedDate;
        }

        public async Task<TaskViewModel> GetTaskById(Guid taskId)
        {
            var task = await repo.GetByIdAsync<Infrastructure.Data.Task>(taskId);

            var taskViewModel = new TaskViewModel()
            {
                Id = taskId,
                Description = task.Description,
                Priority = PriorityCalculator(task.DueDate, task.IsCritical, task.IsCompleted),
                DueDate = task.DueDate,
                IsCritical = task.IsCritical,
                IsCompleted = task.IsCompleted
            };

            return taskViewModel;
        }

        public PriorityLevel PriorityCalculator(string dueDate, bool isCritical , bool isCompleated)
        {
            DateTime.TryParse(dueDate, out DateTime parsedDate);
            DateTime now = DateTime.UtcNow; // UtcNow to avoid timezone issues
            TimeSpan difference = parsedDate - now;

            if(isCompleated)
            {
                return PriorityLevel.Low;
            }
            else if((difference.TotalDays <= TaskServiceConstants.Task_High_Priority_Day_Minimum && difference.TotalDays >= 0) || isCritical)
            {
                return PriorityLevel.High;
            }
            else
            {
                return PriorityLevel.Medium;
            }
        }

        public async Task<bool> RemoveTaskById(Guid taskId)
        {
            if (await repo.GetByIdAsync<Infrastructure.Data.Task>(taskId) == null)
            {
                return false;
            }

            await repo.DeleteAsync<Infrastructure.Data.Task>(taskId);
            await repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpgateTask(TaskViewModel taskViewModel)
        {
            var task = await repo.GetByIdAsync<Infrastructure.Data.Task>(taskViewModel.Id);

            if(task == null)
            {
                return false;
            }

            task.Id = taskViewModel.Id;
            task.Description = taskViewModel.Description;
            task.Priority = PriorityCalculator(taskViewModel.DueDate, taskViewModel.IsCritical, taskViewModel.IsCompleted);
            task.DueDate = taskViewModel.DueDate;
            task.IsCritical = taskViewModel.IsCritical;
            task.IsCompleted = taskViewModel.IsCompleted;

            repo.Update(task);
            await repo.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TaskViewModel>> FilterTasksByCompleated(IEnumerable<TaskViewModel> tasks, bool isCompleated)
        {
            return tasks.Where(task => task.IsCompleted == isCompleated);
        }
    }
}
