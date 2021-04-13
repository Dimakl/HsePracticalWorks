using System;
using System.Collections.Generic;

namespace TaskManagement
{
    public abstract class AbstractSubtask : AbstractTask, IAssignable
    {
        public int assignedLimit;

        public AbstractSubtask(TaskStatus taskStatus, string taskName, string taskDescription, DateTime taskDate, int assignedLimit) :
            base(taskStatus, taskName, taskDescription, taskDate)
        {
            this.assignedLimit = assignedLimit;
            Assigned = new List<User>();
        }
        
        public List<User> Assigned { get; set; }

        public uint Limit { get; set; }

        public bool AddAssigned(User user)
        {
            if (!Assigned.Contains(user))
                return false;
            if (Assigned.Count == assignedLimit)
                return false;
            Assigned.Add(user);
            return true;
        }

        public bool DeleteAssigned(User user)
        {
            if (!Assigned.Contains(user))
                return false;
            Assigned.Remove(user);
            return true;
        }

        private string GetAssigned()
        {
            if (Assigned.Count == 0)
                return "Исполнителей не назначено";
            return string.Join(" ", Assigned);

        }

        public override string ToString()
        {
            return $"{taskName},\nТип задачи - {taskType.Value},\nДата создания - {taskDate.ToString("MM/dd/yy H:mm:ss zzz")}\n" +
                $"Статус задачи - {taskStatus.Value}, Список исполнителей задачи: {GetAssigned()}\nМаксмальное количество исполнителей - {assignedLimit}:\n{taskDescription}\n";
        }
    }
}
