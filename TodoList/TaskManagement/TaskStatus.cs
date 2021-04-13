using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public class TaskStatus
    {
        public string Value { get; set; }
        private TaskStatus(string value)
        {
            Value = value;
        }

        public static TaskStatus Opened => new TaskStatus("Открытая задача");
        public static TaskStatus InWork => new TaskStatus("Задача в работе");
        public static TaskStatus Done => new TaskStatus("Завершенная задача");

    }
}
