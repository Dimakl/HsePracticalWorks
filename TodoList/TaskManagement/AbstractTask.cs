using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public abstract class AbstractTask
    {
        public TaskStatus taskStatus;
        public string taskName;
        public string taskDescription;
        public DateTime taskDate;
        public TaskType taskType;

        public AbstractTask(TaskStatus taskStatus, string taskName, string taskDescription, DateTime taskDate)
        {
            this.taskStatus = taskStatus;
            this.taskName = taskName;
            this.taskDescription = taskDescription;
            this.taskDate = taskDate;
        }

        public override string ToString()
        {
            return $"{taskName},\nТип задачи - {taskType.Value},\nДата создания - {taskDate:MM/dd/yy H:mm:ss zzz}\n" +
                $"Статус задачи - {taskStatus.Value},\n{taskDescription}\n";
        }
    }
}
