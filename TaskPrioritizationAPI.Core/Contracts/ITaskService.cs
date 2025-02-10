using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPrioritizationAPI.Core.Models;
using TaskPrioritizationAPI.Infrastructure.Data.Common;

namespace TaskPrioritizationAPI.Core.Contracts
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskViewModel>> OrderTasksByPriority(IEnumerable<TaskViewModel> tasks);

        Task<IEnumerable<TaskViewModel>> OrderTasksByDueDate(IEnumerable<TaskViewModel> tasks);

        Task<IEnumerable<TaskViewModel>> GetAllTasks();

        Task<IEnumerable<TaskViewModel>> FilterTasksByCompleated(IEnumerable<TaskViewModel> tasks, bool isCompleated);

        Task<TaskViewModel> AddTask(TaskAddViewModel taskAddModel);

        Task<bool> RemoveTaskById(Guid taskId);

        Task<TaskViewModel> GetTaskById(Guid taskId);

        Task<bool> UpgateTask(TaskViewModel taskViewModel);

        PriorityLevel PriorityCalculator(string dueDate , bool isCritical, bool isCompleated);
    }
}
