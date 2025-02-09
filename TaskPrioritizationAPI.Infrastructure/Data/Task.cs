using TaskPrioritizationAPI.Infrastructure.Data.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPrioritizationAPI.Infrastructure.Data.Common;

namespace TaskPrioritizationAPI.Infrastructure.Data
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(DatabaseConstants.Task_Description_Max_Length)]
        public string Title { get; set; }

        [Required]
        [StringLength(DatabaseConstants.Task_Description_Max_Length)]
        public string Description { get; set; }

        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

        [Required]
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
