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
        Task<IEnumerable<TaskViewModel>> GetAllTasksByPriority();

        Task<IEnumerable<TaskViewModel>> GetAllUncompleatedTasks();

        Task<TaskViewModel> AddTask(TaskAddViewModel taskAddModel);

        Task<bool> RemoveTask(Guid taskId);

        Task<TaskViewModel> GetTaskById(Guid taskId);

        Task<bool> UpgateTask(TaskViewModel taskViewModel);

        Task<PriorityLevel> PriorityCalculator(string dueDate , bool isCritical, bool isCompleated);
    }
}
