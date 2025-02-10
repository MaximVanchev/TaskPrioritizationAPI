using Microsoft.AspNetCore.Mvc;
using TaskPrioritizationAPI.Core.Contracts;
using TaskPrioritizationAPI.Core.Models;
using TaskPrioritizationAPI.Core.Services;
using TaskPrioritizationAPI.Infrastructure.Data.Common;

namespace TaskPrioritizationAPI.Controllers
{
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TaskController(ITaskService _taskService)
        {
            taskService = _taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskAddViewModel? task)
        {
            if (task == null)
            {
                return BadRequest("Invalid input. Enter task.");
            }

            if (!string.IsNullOrEmpty(task.DueDate) && !DateTime.TryParse(task.DueDate, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format. Use YYYY-MM-DD.");
            }

            if(task.Title == null || task.Description == null)
            {
                return BadRequest("Title or description are not found.Please enter them.");
            }

            return Ok(await taskService.AddTask(task));
        }

            [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] string? sort, [FromQuery] string? filter, [FromQuery] string? value)
        {
            var tasks = await taskService.GetAllTasks(); // Retrieve tasks from DB

            // Apply Filtering
            if (!string.IsNullOrEmpty(filter) && !string.IsNullOrEmpty(value))
            {
                switch (filter.ToLower())
                {
                    case "iscompleted":
                        if (bool.TryParse(value, out bool isCompleted))
                        {
                            tasks = await taskService.FilterTasksByCompleated(tasks , isCompleted);
                        }
                        else
                        {
                            return BadRequest("Invalid value for isCompleted. Use 'true' or 'false'.");
                        }
                        break;
                    default:
                        return BadRequest("Invalid filter parameter. Use 'isCompleted'.");
                }
            }

            // Apply Sorting
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "priority":
                        tasks = tasks = await taskService.OrderTasksByPriority(tasks);
                        break;
                    case "duedate":
                        tasks = tasks = await taskService.OrderTasksByDueDate(tasks);
                        break;
                    default:
                        return BadRequest("Invalid sort parameter. Use 'priority' or 'dueDate'.");
                }
            }

            return Ok(tasks);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskViewModel updatedTask)
        { 

            if (await taskService.UpgateTask(updatedTask) == false)
            {
                return NotFound($"The task is not found or couldn't be updated.");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {

            if (await taskService.RemoveTaskById(id) == false)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            return NoContent(); // 204 No Content (successful deletion)
        }
    }
}
