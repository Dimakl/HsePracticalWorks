using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public class Story : AbstractSubtask
    {
        public Story(TaskStatus taskStatus, string taskName, string taskDescription, DateTime taskDate) : base(taskStatus, taskName, taskDescription, taskDate, 5)
        {
            taskType = TaskType.Story;
        }
    }
}
