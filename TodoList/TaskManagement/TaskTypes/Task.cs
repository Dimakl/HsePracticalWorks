using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public class Task : AbstractSubtask
    {
        public Task(TaskStatus taskStatus, string taskName, string taskDescription, DateTime taskDate) : base(taskStatus, taskName, taskDescription, taskDate, 1)
        {
            taskType = TaskType.Task;
        }
    }
}
