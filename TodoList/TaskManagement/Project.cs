using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public class Project
    {
        public string Name { get; set; }
        public List<AbstractTask> Tasks { get; set; }
        public int Limit { get; set; }



        public static List<Project> projects;

        static Project()
        {
            projects = new List<Project>();
        }

        private Project(string name)
        {
            Limit = 10;
            Tasks = new List<AbstractTask>();
            Name = name;
        }

        public static void AddProject(string name)
        {
            projects.Add(new Project(name));
        }

        public static bool DeleteProject(int num)
        {
            if (projects.Count - 1 < num)
                return false;
            projects.RemoveAt(num);
            return true;
        }

        public static void PrintProject()
        {
            for (int i = 0; i < projects.Count; i++)
                Console.WriteLine($"Индекс проекта - {i}, Имя проекта - {projects[i].Name}, Количество задач - {projects[i].Tasks.Count}," +
                    $" Лимит задач - {projects[i].Limit}");
        }

        public static bool ChangeName(int num, string newName)
        {
            if (projects.Count - 1  < num)
                return false;
            projects[num].Name = newName;
            return true;
        }

        public static bool SelectProject(out Project project)
        {
            PrintProject();
            project = null;
            Console.WriteLine("Введите индекс нужного проекта");
            string name = Console.ReadLine();
            if (!int.TryParse(name, out int a))
            {
                Console.WriteLine("Было введено не число");
                return false;
            }
            if (projects.Count - 1 < a)
                return false;
            project = projects[a];
            return true;
        }
    }
}
