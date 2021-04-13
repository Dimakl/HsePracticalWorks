using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Warehouse.Exceptions;

namespace Warehouse
{

    // Additional functionality is addbox function to addboxes to existing containers.
    class Program
    {


        /// <summary>
        /// Enum containing which mehod of usage app you would choose.
        /// </summary>
        enum InputType { File, Console };



        /// <summary>
        /// Main mehod. Is launched.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool flg = true;
            do
            {
                try
                {
                    var inputType = GetInputType();
                    if (flg)
                        InputWarehouse(inputType);
                    if (inputType == InputType.Console)
                        while (WarehouseConsoleCycle()) ;
                    else
                        WarehouseFileCycle();
                    flg = false;
                    Warehouse.GetWarehouse().ClearWarehouse();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Some problem interrupted your usage of this app, sorry :/");
                }
            }
            while (UserWantsToContinue());
        }


        private const string textForFileCycle = "List of possible actions with your warehouse (write them one on each line in file that you would specify later):\n" +
                "add - you add a next container from your container file to current container list\n" +
                "addbox 'number of container' 'weight of box' 'cost for kilo of box' - you add box to a specified container, (it would not be damaged because its added as a new one, it's jus tnot that old :) \n" +
                "delete 'number of container' - deletes container with specified id from the warehouse\n" +
                "info 'filename' or info - prints all your warehouse information to the constole or to the specified file" +
                "quit - prints warehouse information and clears current container info from it, breaks from other actions";



        /// <summary>
        /// Cycle which is launched when type of inputing data is from file.
        /// </summary>
        private static void WarehouseFileCycle()
        {
            Console.WriteLine(textForFileCycle);
            Console.WriteLine("Now input your file with actions:");
            string actionsPath = ReadValue(TryToReadFromFile);
            Console.WriteLine("Now input your file with containers:");
            string containersPath = ReadValue(TryToReadFromFile);
            string[] actions = File.ReadAllLines(actionsPath);
            string[] containers = File.ReadAllLines(containersPath);
            int currentContainer = 0;
            foreach (var action in actions)
            {
                Console.WriteLine($"proceeding action:\n * {action}");
                if (action == "add")
                {
                    Console.WriteLine($"working with container input:\n * {containers[currentContainer]}");
                    AddContainerFromFile(containers, currentContainer);
                    currentContainer++;
                }
                else if (action.StartsWith("delete"))
                {
                    DeleteContainerWithFileInfo(action);
                }
                else if (action.StartsWith("addbox") && action.Length == 4)
                {
                    AddBox(action.Split().Skip(1).Take(3).ToList());
                }
                else if (action.StartsWith("info"))
                {
                    PrintWarehouseInfo(action.Length == 4 ? "---" : action.Substring(5, action.Length - 5));
                }
                else if (action == "quit")
                {
                    PrintWarehouseInfo();
                    return;
                }
                else
                    Console.WriteLine("Unnowk or wrong-written command");
            }
        }


        /// <summary>
        /// Main method for adding box to existing container.
        /// </summary>
        /// <param name="args"> Params specified in command: 'number of container' 'weight of box' 'cost for kilo of box'. </param>
        private static void AddBox(List<string> args)
        {
            bool flg = true;
            if (!int.TryParse(args[0], out int tmp) || tmp < 0)
                flg = false;
            foreach (var el in args.Skip(1).Take(2))
                if (!double.TryParse(el, out double tmp1) || tmp1 < 0)
                    flg = false;
            if (!flg)
            {
                Console.WriteLine("Wrong input in addbox");
                return;
            }
            Box box = new Box(double.Parse(args[1]), double.Parse(args[2]));
            try
            {
                Warehouse.GetWarehouse().AddBox(int.Parse(args[0]), box);
                Console.WriteLine($"New box:\n{box}Was added to container #{args[0]}");
            }
            // Not specifiying whitch execption is it because it could be ContainerOverweightException or ArgumentException from Warehouse class.
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Deletes container which input of file type.
        /// </summary>
        /// <param name="action"> Action string.</param>
        private static void DeleteContainerWithFileInfo(string action)
        {
            if (action.Split().Length == 2 && int.TryParse(action.Split()[1], out int num))
            {
                Warehouse.GetWarehouse().DeleteContainer(num);
            }
            else
            {
                Console.WriteLine($"this line of action is written in wrong format:\n {action}");
            }
        }


        /// <summary>
        ///  Adds container using input of file type.
        /// </summary>
        /// <param name="containers"> Containers file content. </param>
        /// <param name="currentContainer">Container that will be used in this operation. </param>
        private static void AddContainerFromFile(string[] containers, int currentContainer)
        {
            if (CheckContainerInput(containers[currentContainer]))
                AddToWarehouse(containers[currentContainer].Split());
            else
                Console.WriteLine("Line with container input was unreadable beacuse of its content");
        }


        /// <summary>
        /// Checks if there is such file that user specifies
        /// </summary>
        /// <param name="input"> Input filepath that user specifies. </param>
        /// <returns>True f data can be readen from file.</returns>
        private static bool TryToReadFromFile(string input)
        {
            try
            {
                string f = File.ReadAllText(input);
                return f != "";
            }
            catch (Exception e)
            {
                return false;
            }
        }


        /// <summary>
        /// Cycle that is repeated for actions inputed from console.
        /// </summary>
        /// <returns>Should cycle be resumed.</returns>
        private static bool WarehouseConsoleCycle()
        {
            Console.WriteLine("List of possible actions with your warehouse:\n" +
                "add - you add a container to current container list, you would need to specify containers list of boxes, after you input this\n" +
                "addbox - you add box to a specified container, than you would need to enter 'number of container' 'weight of box' " +
                "'cost for kilo of box', (it would not be damaged because its added as a new one, it's jus tnot that old :) )\n" +
                "delete - deletes container with specified id from the warehouse, you would need to enter container number\n" +
                "info - prints all your warehouse information to the constole " +
                "quit - prints warehouse information and clears current container info from it");
            string input = ReadValue(s => (new HashSet<string> { "add", "delete", "info", "quit", "addbox" }).Contains(s));
            switch (input)
            {
                case "add":
                    AddConsoleToWarehouse();
                    return true;
                case "addbox":
                    AddBoxConsole();
                    return true;
                case "delete":
                    DeleteConsoleFromWarehouse();
                    return true;
                case "info":
                    PrintWarehouseInfo();
                    return true;
                case "quit":
                    PrintWarehouseInfo();
                    return false;
                default:
                    return false;
            }
        }


        /// <summary>
        /// Prepares data from console to be used in AddBox method.
        /// </summary>
        private static void AddBoxConsole()
        {
            Console.WriteLine("Input container num >= 0, box weight >= 0 and cost for kilo >=0 in one line:");
            string input = ReadValue(ValidateAddBoxInput);
            AddBox(input.Split().ToList());
        }


        /// <summary>
        /// Validates that input from console is good to use in AddBox mehtod.
        /// </summary>
        /// <param name="input"> Input by user. </param>
        /// <returns> If this input is valid. </returns>
        private static bool ValidateAddBoxInput(string input)
        {
            string[] s = input.Split();
            if (s.Length != 3)
                return false;
            if (!int.TryParse(s[0], out int tmp) || tmp < 0)
                return false;
            foreach (var el in s.Skip(1).Take(2))
                if (!double.TryParse(el, out double tmp1) || tmp1 < 0)
                    return false;
            return true;
        }


        /// <summary>
        /// Prepares console input to be used in AddToWarehouse method.
        /// </summary>
        private static void AddConsoleToWarehouse()
        {
            Console.WriteLine("Input container variables in one string - split only with space where there are pair values for each box - weight and cost for kilo:");
            string input = ReadValue(CheckContainerInput, "There was a mistake in boxes list for container, try again");
            AddToWarehouse(input.Split());
        }

        /// <summary>
        /// Adds container to the warehouse singleton.
        /// </summary>
        /// <param name="input"> Input that would be added. </param>
        private static void AddToWarehouse(string[] input)
        {
            List<Box> boxes = new List<Box>(0);
            for (int i = 0; i < input.Length; i += 2)
                boxes.Add(new Box(double.Parse(input[i]), double.Parse(input[i + 1])));

            Container container;
            try
            {
                container = new Container(boxes);
            }
            catch (ContainerOverweightException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            try
            {
                Warehouse.GetWarehouse().AddContainer(container);
            }
            catch (UnprofitableContainerException e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Is this input valid for creating a container.
        /// </summary>
        /// <param name="s"> Input that is checked. </param>
        /// <returns> If it is valid. </returns>
        private static bool CheckContainerInput(string s)
        {
            if (s.Split().Length % 2 == 1)
                return false;
            foreach (var el in s.Split())
                if (!double.TryParse(el, out double _))
                    return false;
            return true;
        }


        /// <summary>
        /// Prepares console input to be used in deletion from the warehouse.
        /// </summary>
        private static void DeleteConsoleFromWarehouse()
        {
            Console.WriteLine("Input container # you want to delete:");
            string input = ReadValue(s => int.TryParse(s, out int _));
            Warehouse.GetWarehouse().DeleteContainer(int.Parse(input));
        }


        /// <summary>
        /// Prints warehouse info to given file, or to the console.
        /// </summary>
        /// <param name="filename"> Optional file to print info to. </param>
        private static void PrintWarehouseInfo(string filename = "---")
        {
            if (filename == "---")
                Console.WriteLine(Warehouse.GetWarehouse());
            else
                File.WriteAllText(filename, Warehouse.GetWarehouse().ToString());
        }


        /// <summary>
        /// Method for the creation of the warehouse singleton.
        /// </summary>
        /// <param name="inputType"> How would be warehouse information be inputed. </param>
        private static void InputWarehouse(InputType inputType)
        {
            Console.WriteLine("Your warehouse information must be values written in one line,\n" +
                "Where first is cost of containing container in warehouse which is >= 0,\n" +
                "And second is amount of containers that can be kept in warehouse its real number >= 1");

            string input;
            if (inputType == InputType.File)
            {
                Console.WriteLine("Input file where warehouse info is kept:");
                input = ReadValue(path => IsWarehouseInputCorrect(File.ReadAllText(path)), "Wrong file name, please try again");
                input = File.ReadAllText(input);
                Console.WriteLine($"contents of warehouse file:\n * {input}");
            }
            else
            {
                Console.WriteLine("Input warehouse info:");
                input = ReadValue(input => IsWarehouseInputCorrect(input));
            }
            if (ParseWarehouseInput(input, out double storageCost, out int containerLimit))
                Warehouse.GetWarehouse(storageCost, containerLimit);
            else
                Console.WriteLine("Not valid data given to warehouse");
        }


        /// <summary>
        /// Is input fiven by user to create warehouse correct.
        /// </summary>
        /// <param name="input"> Input to be checked. </param>
        /// <returns> If it's correct. </returns>
        private static bool IsWarehouseInputCorrect(string input)
        {
            string[] split = input.Split();
            if (split.Length != 2)
                return false;
            return ParseWarehouseInput(input, out double _, out int _);
        }


        /// <summary>
        /// Parses warehouse input if it's syntactically right.
        /// </summary>
        /// <param name="input"> Input.</param>
        /// <param name="storageCost"> Storage cost. </param>
        /// <param name="containerLimit"> Container limit. </param>
        /// <returns></returns>
        private static bool ParseWarehouseInput(string input, out double storageCost, out int containerLimit)
        {
            string[] splitInput = input.Split();
            storageCost = 0;
            containerLimit = 0;
            return double.TryParse(splitInput[0], out storageCost) && storageCost >= 0
                && int.TryParse(splitInput[1], out containerLimit) && containerLimit > 0;
        }


        /// <summary>
        /// Gets type of input from user.
        /// </summary>
        /// <returns> Element of enum that shows input type. </returns>
        private static InputType GetInputType()
        {
            Console.WriteLine("Welcome to Warehouse Manager!\n" +
                "You should type in how you would be using this app\n" +
                "Type \"console\" to make all input from console\n" +
                "Or type in \"file\" to make your input from file");
            string inp = ReadValue(inp => (new HashSet<string> { "console", "file" }).Contains(inp));
            return inp == "console" ? InputType.Console : InputType.File;
        }


        /// <summary>
        /// Check if user wants to continue or end the process.
        /// </summary>
        /// <returns> Does user want it. </returns>
        private static bool UserWantsToContinue()
        {
            Console.WriteLine("type in quit if you want to end program, and anything else to use Warhouse Manager again");
            return Console.ReadLine() != "quit";
        }


        /// <summary>
        /// Universal method for reading input from user and verifiying that its correct.
        /// </summary>
        /// <param name="predicate"> Predicate for verifiying. </param>
        /// <param name="inputError"> Error message to be shown. </param>
        /// <returns> Valid input. </returns>
        private static string ReadValue(Predicate<string> predicate, string inputError = "Wrong input, please try again")
        {
            string inp = "";
            bool flg = false;
            do
            {
                if (flg)
                    Console.WriteLine(inputError);
                inp = Console.ReadLine();
                flg = true;
            }
            while (!predicate(inp));
            return inp;
        }
    }
}
