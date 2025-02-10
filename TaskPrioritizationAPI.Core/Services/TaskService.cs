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
            var task = new TaskViewModel()
            {
                Title = taskAddModel.Title,
                Description = taskAddModel.Description,
                Priority = await PriorityCalculator(taskAddModel.DueDate, taskAddModel.IsCritical, taskAddModel.IsCompleted),
                DueDate = taskAddModel.DueDate,
                IsCritical = taskAddModel.IsCritical,
                IsCompleted = taskAddModel.IsCompleted
            };
            
            await repo.AddAsync(task);
            await repo.SaveChangesAsync();

            return task;
        }

        public async Task<IEnumerable<TaskViewModel>> GetAllTasksByPriority()
        {
            var tasks = repo.All<Infrastructure.Data.Task>().Select(t => new TaskViewModel()
            { 
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                DueDate = t.DueDate,
                IsCompleted= t.IsCompleted,
                IsCritical= t.IsCritical
            });

            foreach (var task in tasks)
            {
                task.Priority = await PriorityCalculator(task.DueDate, task.IsCritical, task.IsCompleted);
            }

            return tasks.OrderByDescending(t => t.Priority);

        }

        public async Task<IEnumerable<TaskViewModel>> GetAllUncompleatedTasks()
        {
            return GetAllTasksByPriority().Result.Where(t => t.IsCompleted == true);
        }

        public async Task<TaskViewModel> GetTaskById(Guid taskId)
        {
            var task = await repo.GetByIdAsync<Infrastructure.Data.Task>(taskId);

            var taskViewModel = new TaskViewModel()
            {
                Id = taskId,
                Description = task.Description,
                Priority = await PriorityCalculator(task.DueDate, task.IsCritical, task.IsCompleted),
                DueDate = task.DueDate,
                IsCritical = task.IsCritical,
                IsCompleted = task.IsCompleted
            };

            return taskViewModel;
        }

        public async Task<PriorityLevel> PriorityCalculator(string dueDate, bool isCritical , bool isCompleated)
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

        public async Task<bool> RemoveTask(Guid taskId)
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
            task.Priority = await PriorityCalculator(taskViewModel.DueDate, taskViewModel.IsCritical, taskViewModel.IsCompleted);
            task.DueDate = taskViewModel.DueDate;
            task.IsCritical = taskViewModel.IsCritical;
            task.IsCompleted = taskViewModel.IsCompleted;

            repo.Update(task);
            await repo.SaveChangesAsync();

            return true;
        }
    }
}
