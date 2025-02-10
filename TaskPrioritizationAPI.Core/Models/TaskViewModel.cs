using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPrioritizationAPI.Infrastructure.Data.Common;
using TaskPrioritizationAPI.Infrastructure.Data.Constants;

namespace TaskPrioritizationAPI.Core.Models
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PriorityLevel Priority { get; set; }

        public string DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsCritical { get; set; }
    }
}
