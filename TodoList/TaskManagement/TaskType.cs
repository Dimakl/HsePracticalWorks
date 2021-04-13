using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public class TaskType
    {
        public string Value { get; set; }
        private TaskType(string value)
        {
            Value = value;
        }

        public static TaskType Epic => new TaskType("Тема");
        public static TaskType Story => new TaskType("История");
        public static TaskType Task => new TaskType("Задача");
        public static TaskType Bug => new TaskType("Ошибка");
    }
}
