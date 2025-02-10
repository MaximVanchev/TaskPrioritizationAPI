using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPrioritizationAPI.Infrastructure.Data.Common;

namespace TaskPrioritizationAPI.Core.Models
{
    public class TaskAddViewModel
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public string DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsCritical { get; set; }
    }
}
