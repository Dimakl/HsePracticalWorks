using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public class Epic : AbstractSubtask
    {
        public Epic(TaskStatus taskStatus, string taskName, string taskDescription, DateTime taskDate) : base(taskStatus, taskName, taskDescription, taskDate, 5)
        {
            taskType = TaskType.Epic;
        }

        public List<AbstractSubtask> subtasks;

        public bool AddTask(AbstractSubtask task)
        {
            if (task.taskType == TaskType.Bug || task.taskType == TaskType.Epic)
                return false;
            subtasks.Add(task);
            return true;
        }

        public override string ToString()
        {
            return base.ToString() + $"Список подзадач: {string.Join("---------------------\n", subtasks)}\n";
        }
    }
}
