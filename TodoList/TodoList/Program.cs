using System;
using TaskManagement;

namespace TodoList
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
                ProcessComands(Console.ReadLine());
        }

        static void ProcessComands(string command)
        {
            switch (command)
            {
                case "users":
                    ProcessUserCommands();
                    break;
                case "projects":
                    ProcessProjectsCommands();
                    break;
                case "tasks":
                    if (!Project.SelectProject(out Project project))
                    {
                        Console.WriteLine("Неверный ввод");
                        break;
                    }
                    ProcessTasksCommands(project);
                    break;
                default:
                    Console.WriteLine("Такой команды нет, попробуйте еще раз");
                    break;
            }
        }

        private static void ProcessTasksCommands(Project project)
        {
            Console.WriteLine("Введите команду из перечисленных:\ncreate - создание задачи\nshow - список задач\nsort - сортировка задач,\nselect - выбрать задачу для изменения\nquit - убрать выбор текущего проекта");
            string command = Console.ReadLine();
            while (command != "quit")
            {
                switch (command)
                {
                    case "create":
                        CreateTask(project);
                        break;
                    case "show":
                        Console.WriteLine("Список задач:");
                        for (int i = 0; i < project.Tasks.Count; i++)
                            Console.WriteLine($"Задача с индексом {i}:\n{project.Tasks[i]}");
                        break;
                    case "sort":
                        ShowSortedTasks(project);
                        break;
                    case "select":
                        Console.WriteLine("Список задач:");
                        for (int i = 0; i < project.Tasks.Count; i++)
                            Console.WriteLine($"Задача с индексом {i}:\n{project.Tasks[i]}");
                        Console.WriteLine("Напишите индекс задачи:");
                        if (!int.TryParse(Console.ReadLine(), out int a) || a >= project.Tasks.Count)
                        {
                            Console.WriteLine("Введен неверный индекс");
                            return;
                        }
                        ProcessComandsForSelectedTask(project, a);
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
                Console.WriteLine("Введите команду из перечисленных:\ncreate - создание задачи\nshow - список задач\nsort - сортировка задач,\nselect - выбрать задачу для изменения\nquit - убрать выбор текущего проекта");
                command = Console.ReadLine();
            }
        }

        private static void ProcessComandsForSelectedTask(Project project, int ind)
        {
            Console.WriteLine($"Выбранная задача - {project.Tasks[ind]}");
            Console.WriteLine("Введите команду из перечисленных:\ncreate - создание задачи\nshow - список задач\nsort - сортировка задач,\nselect - выбрать задачу для изменения\nquit - убрать выбор текущего проекта");
        }

        private static void ShowSortedTasks(Project project)
        {
            Console.WriteLine("Список задач (отсортированный по текущему статусу - opened -> inwork -> done):");
            for (int i = 0; i < project.Tasks.Count; i++)
                if (project.Tasks[i].taskStatus == TaskStatus.Opened)
                    Console.WriteLine($"Задача с индексом {i}:\n{project.Tasks[i]}");
            for (int i = 0; i < project.Tasks.Count; i++)
                if (project.Tasks[i].taskStatus == TaskStatus.InWork)
                    Console.WriteLine($"Задача с индексом {i}:\n{project.Tasks[i]}");
            for (int i = 0; i < project.Tasks.Count; i++)
                if (project.Tasks[i].taskStatus == TaskStatus.Done)
                    Console.WriteLine($"Задача с индексом {i}:\n{project.Tasks[i]}");
        }

        private static void CreateTask(Project project)
        {
            if (project.Limit == project.Tasks.Count)
            {
                Console.WriteLine("Проект переполнен, невозможно добавить задачу");
                return;
            }
            Console.WriteLine("Выберите тип задачи: epic - большая задача с подзадачами, story - задача с объемом работы меньшим чем epic," +
                " task - задача с объемом работы меньшим чем story, bug - ошибка\nВыберите один из вышеперечисленных типов:");
            string type = Console.ReadLine();
            Console.WriteLine("Введите текущий этап выполенения задачи: opened, inwork, done:");
            TaskStatus taskStatus;
            switch (Console.ReadLine())
            {
                case "opened":
                    taskStatus = TaskStatus.Opened;
                    break;
                case "inwork":
                    taskStatus = TaskStatus.InWork;
                    break;
                case "done":
                    taskStatus = TaskStatus.Done;
                    break;
                default:
                    Console.WriteLine("Введен неверный этап выполнения");
                    return;
            }
            Console.WriteLine("Введите имя задачи:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите описание задачи:");
            string desc = Console.ReadLine();
            CreateTaskWithFullInfo(project, type, taskStatus, name, desc);
        }

        private static void CreateTaskWithFullInfo(Project project, string type, TaskStatus taskStatus, string name, string desc)
        {
            switch (type)
            {
                case "epic":
                    project.Tasks.Add(new Epic(taskStatus, name, desc, DateTime.Now));
                    Console.WriteLine($"Задача создана:\n{project.Tasks[^1]}");
                    break;
                case "story":
                    project.Tasks.Add(new Story(taskStatus, name, desc, DateTime.Now));
                    Console.WriteLine($"Задача создана:\n{project.Tasks[^1]}");
                    break;
                case "task":
                    project.Tasks.Add(new Task(taskStatus, name, desc, DateTime.Now));
                    Console.WriteLine($"Задача создана:\n{project.Tasks[^1]}");
                    break;
                case "bug":
                    project.Tasks.Add(new Bug(taskStatus, name, desc, DateTime.Now));
                    Console.WriteLine($"Задача создана:\n{project.Tasks[^1]}");
                    break;
                default:
                    Console.WriteLine("Введен неверный тип задачи");
                    break;
            }
        }

        private static void ProcessProjectsCommands()
        {
            Console.WriteLine("Введите команду из перечисленных:\ncreate - создание проекта\nshow - список проектов\ndelete - удаление проекта,\nchange - изменение имени проекта");
            string command = Console.ReadLine();
            switch (command)
            {
                case "show":
                    Project.PrintProject();
                    break;
                case "create":
                    Console.WriteLine("Введите название нового проекта");
                    string name = Console.ReadLine();
                    Project.AddProject(name);
                    Console.WriteLine($"Проект {name} был добавлен");
                    break;
                case "delete":
                    Project.PrintProject();
                    Console.WriteLine("Введите индекс удаляемого проекта");
                    name = Console.ReadLine();
                    if (!int.TryParse(name, out int a))
                    {
                        Console.WriteLine("Было введено не число");
                        break;
                    }
                    if (!Project.DeleteProject(a))
                    {
                        Console.WriteLine("Такого проекта не существует");
                        break;
                    }
                    Console.WriteLine($"Проект с индексом {name} был удален");
                    break;
                case "change":
                    ChangeFragment(out name, out a);
                    break;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }

        private static void ChangeFragment(out string name, out int a)
        {
            Project.PrintProject();
            Console.WriteLine("Введите индекс изменяемого проекта");
            name = Console.ReadLine();
            if (!int.TryParse(name, out a))
            {
                Console.WriteLine("Было введено не число");
                return;
            }
            Console.WriteLine("Введите новое имя проекта");
            name = Console.ReadLine();
            if (!Project.ChangeName(a, name))
            {
                Console.WriteLine("Такого проекта не существует");
                return;
            }
            Console.WriteLine($"Проект с индексом {a} был изменен");
        }

        private static void ProcessUserCommands()
        {
            Console.WriteLine("Введите команду из перечисленных:\ncreate - создание пользователя\nshow - список пользователей\ndelete - удаление пользователя");
            string command = Console.ReadLine();
            switch (command)
            {
                case "show":
                    Console.WriteLine($"Список всех пользователей: \n{string.Join(", ",User.Users)}");
                    break;
                case "create":
                    Console.WriteLine("Введите имя нового пользователя");
                    string name = Console.ReadLine();
                    User.AddUser(name);
                    Console.WriteLine($"Пользователь {name} был добавлен");
                    break;
                case "delete":
                    Console.WriteLine("Введите имя удаляемого пользователя");
                    name = Console.ReadLine();
                    User.DeleteUser(name);
                    Console.WriteLine($"Все пользователи с именем {name} были удалены");
                    break;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }
    }
}
