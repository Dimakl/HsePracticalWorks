    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Unicode;

    namespace FileManager
    {
        class Program
        {

            // All public methods in this class serve as implementations of each command abstractions.
            // Private methods are used only within this class.
            // Each method documentation could be checked in help dictionary (there are command descriptions and their Bacus-Naur forms) so documenting each methods within code would be redunant.
            static class CommandMethods
            {

                // This method outputs specified info (for each method) by using reflection.
                // Reflection: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/reflection
                public static void NotifyHelp()
                {
                    Console.WriteLine("use \"help\" to check all commands usage (NOTE: use | instead of space: \" \" in directory names, paths, and file names)");
                    StackTrace stackTrace = new StackTrace();
                    string callerName = stackTrace.GetFrame(1).GetMethod().Name;
                    if (commands.ContainsKey(callerName.ToLower()))
                        Help(callerName.ToLower());
                }


            /// <summary>
            /// Help: command that outputs info about all commands and their usage in FileManager. usage:
            /// help [command]
            /// Where command specifies which command info are you looking for.
            /// And without using it you get all commands info.
            /// </summary>
            /// <param name="p"></param>
            public static void Help(params string[] p)
                {
                    if (p.Length == 1)
                        if (commands.ContainsKey(p[0]))
                            Console.WriteLine($"{p[0]}: {commandDescription[p[0]]}");
                        else
                            NotifyHelp();
                    else if (p.Length == 0)
                        foreach (var command in commands.Keys)
                            Console.WriteLine($"{command}: {commandDescription[command]}\n");
                    else
                        NotifyHelp();
                }


            /// <summary>
            /// Diskinfo: command that prints info about your disks. usage:
            /// diskinfo [diskName]...
            /// Where disk prints all disks' info, while specifiying disk names prints only their info in format that disks are named when using "diskinfo".
            /// </summary>
            /// <param name="p"></param>
            public static void DiskInfo(params string[] p)
                {
                    DriveInfo[] allDrives = DriveInfo.GetDrives();
                    Dictionary<string, DriveInfo> driveDictionary = new Dictionary<string, DriveInfo>();
                    foreach (var d in allDrives)
                        if (d.IsReady)
                            driveDictionary.Add(d.Name, d);
                    if (p.Length == 0)
                    {
                        foreach (var d in allDrives)
                            if (d.IsReady)
                                Console.WriteLine($"Drive name - {d.Name}\n Available free space: {d.AvailableFreeSpace}");
                    }
                    else
                        foreach (var param in p)
                            if (driveDictionary.ContainsKey(param))
                                Console.WriteLine($"Drive name - {driveDictionary[param]}\nAvailable free space: {driveDictionary[param].AvailableFreeSpace} bytes\n" +
                                    $"File system: {driveDictionary[param].DriveFormat}\nTotal available space: {driveDictionary[param].TotalFreeSpace} bytes");
                            else
                                Console.WriteLine($"No info about such drive as {param}");
                }


            /// <summary>
            /// Disk: command that sets current disk to specified. usage:
            /// disk <diskName>
            /// Where diskName is obviously disk name.
            /// </summary>
            /// <param name="p"></param>
            public static void Disk(params string[] p)
                {
                    DriveInfo[] allDrives = DriveInfo.GetDrives();
                    Dictionary<string, DriveInfo> driveDictionary = new Dictionary<string, DriveInfo>();
                    foreach (var d in allDrives)
                        if (d.IsReady)
                            driveDictionary.Add(d.Name, d);
                    if (p.Length == 1)
                        if (driveDictionary.ContainsKey(p[0]))
                        {
                            currentPath = driveDictionary[p[0]].Name;
                            Console.WriteLine($"current disk is {p[0]}");
                        }
                        else
                            Console.WriteLine($"No info about such drive as {p[0]}");
                    else
                        NotifyHelp();
                }


            /// <summary>
            /// Inner method for thois class.
            /// </summary>
            /// <param name="path"></param>
            private static void PrintDirectoryFilesAndDirectories(string path)
                {
                    Console.WriteLine($"Files in {path}");
                    foreach (var file in Directory.GetFiles(path))
                        Console.WriteLine(file);
                    Console.WriteLine($"\nDirectories in {path}");
                    foreach (var directory in Directory.GetDirectories(path))
                        Console.WriteLine(directory);
                }


            /// <summary>
            /// Ls: prints files, contained within directory. usage:
            /// ls [path]
            /// By default shows files in current directory, but with specified path - within it.
            /// </summary>
            /// <param name="p"></param>
            public static void Ls(params string[] p)
                {
                    if (p.Length == 0)
                    {
                        PrintDirectoryFilesAndDirectories(currentPath);
                    }
                    else if (p.Length == 1)
                    {
                        string fullPath;
                        try
                        {
                            fullPath = Path.GetFullPath(p[0]);
                            Directory.GetFiles(fullPath);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"filepath {p[0]} is not valid:\n{e.Message}");
                            return;
                        }
                        PrintDirectoryFilesAndDirectories(p[0]);
                    }
                    else
                        NotifyHelp();
                }


            /// <summary>
            /// Cd: enters the specified directory. usage:
            /// cd [directory]
            /// Where directory could be ".." to go up one level.
            /// </summary>
            /// <param name="p"></param>
            public static void Cd(params string[] p)
                {
                    if (p.Length == 1)
                    {
                        if (p[0] == "..")
                        {
                            CdOut();
                        }
                        else
                        {
                            CdIn(p);
                        }
                    }
                    else
                        NotifyHelp();
                }


                /// <summary>
                /// Inner method for Cd().
                /// </summary>
                private static void CdOut()
                {
                    var parent = Directory.GetParent(currentPath);
                    if (parent == null)
                    {
                        Console.WriteLine($"you cannot use .. in this path: {currentPath}");
                        return;
                    }
                    currentPath = parent.FullName;
                    Console.WriteLine($"current path is now {currentPath}");
                }


            /// <summary>
            /// Inner method for Cd().
            /// </summary>
            /// <param name="p"></param>
            private static void CdIn(string[] p)
                {
                    string fullPath = p[0];
                    try
                    {
                        if (currentPath[currentPath.Length - 1] != Path.DirectorySeparatorChar)
                            currentPath = currentPath + Path.DirectorySeparatorChar;
                        fullPath = Path.GetFullPath(currentPath + p[0]);
                        Directory.GetFiles(fullPath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"filepath {fullPath} is not valid:\n{e.Message}");
                        return;
                    }
                    currentPath = fullPath;
                    Console.WriteLine($"current path is now {currentPath}");
                }


            /// <summary>
            /// Cat: command shows the content. usage:
            /// cat <fileName> [encoding]
            /// Where encoding is encoding in which content will be shown (default is UTF-8), and fileName is name of the file in the current path.
            /// </summary>
            /// <param name="p"></param>
            public static void Cat(params string[] p)
                {
                    var encoding = Encoding.GetEncoding("utf-8");
                    if (p.Length > 2)
                    {
                        NotifyHelp();
                        return;
                    }
                    if (p.Length == 2)
                        try
                        {
                            encoding = Encoding.GetEncoding(p[1]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"{p[1]} encoding is not supported:\n{e.Message}");
                            return;
                        }
                    string fullPath = currentPath;
                    if (currentPath[currentPath.Length - 1] != Path.DirectorySeparatorChar)
                        fullPath = currentPath + Path.DirectorySeparatorChar;
                    fullPath = fullPath + p[0];
                    try
                    {
                        Console.WriteLine(File.ReadAllText(fullPath, encoding));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"no such file as {fullPath}:\n{e.Message}");
                    }
                }


            /// <summary>
            /// Cp: method for copiying file into another directory. usage:
            /// cp <fileName> <fullNameToDirectoryToCopy>
            /// FileName is relative to current path and fullNameToDirectoryToCopy is absolute.
            /// </summary>
            /// <param name="p"></param>
            public static void Cp(params string[] p)
                {
                    if (p.Length != 2)
                    {
                        NotifyHelp();
                        return;
                    }
                    string fullPath = currentPath + Path.DirectorySeparatorChar + p[0];
                    string fullPathToCopy = p[1] + Path.DirectorySeparatorChar + p[0];
                    try
                    {
                        File.Copy(fullPath, fullPathToCopy);
                        Console.WriteLine($"file {fullPath} was copied to {fullPathToCopy}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"there was a problem copying {fullPath} to {fullPathToCopy}:\n{e.Message}");
                    }
                }


            /// <summary>
            /// Mv: method for moving file into another directory. usage:
            /// mv <fileName> <pathToDirectoryToMove>
            /// FileName is relative to current path and pathToDirectoryToMove is absolute.
            /// </summary>
            /// <param name="p"></param>
            public static void Mv(params string[] p)
                {
                    if (p.Length != 2)
                    {
                        NotifyHelp();
                        return;
                    }
                    string fullPath = currentPath;
                    if (currentPath[currentPath.Length - 1] != Path.DirectorySeparatorChar)
                        fullPath = currentPath + Path.DirectorySeparatorChar;
                    fullPath += p[0];
                    string fullPathToMove = p[1] + Path.DirectorySeparatorChar + p[0];
                    try
                    {
                        File.Move(fullPath, fullPathToMove);
                        Console.WriteLine($"file {fullPath} was moved to {fullPathToMove}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"there was a problem moving {fullPath} to {fullPathToMove}:\n{e.Message}");
                    }
                }


            /// <summary>
            /// Rm: method for removing file from system. usage:
            /// rm <fileName>
            /// FileName is file name - this file will be deleted.
            /// </summary>
            /// <param name="p"></param>
            public static void Rm(params string[] p)
                {
                    if (p.Length != 1)
                    {
                        NotifyHelp();
                        return;
                    }
                    string fullPath = currentPath;
                    if (currentPath[currentPath.Length - 1] != Path.DirectorySeparatorChar)
                        fullPath = currentPath + Path.DirectorySeparatorChar;
                    fullPath += p[0];
                    try
                    {
                        File.Delete(fullPath);
                        Console.WriteLine($"file {fullPath} was successfully removed");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"file {fullPath} is unreachable or doesn't exist:\n{e.Message}");
                    }
                }


            /// <summary>
            /// Touch: creates file with set encoding. usage:
            /// touch <fileName> <encoding> <text>...
            /// You MUST set an encoding in order this method to work? for utf-8 it's "utf-8".
            /// All encoding variants are: utf-16, unicodeFFFE, utf-32, utf-32BE, us-ascii, iso-8859-1, utf-7, utf-8.
            /// </summary>
            /// <param name="p"></param>
            public static void Touch(params string[] p)
                {
                    if (p.Length <= 3)
                    {
                        NotifyHelp();
                        return;
                    }
                    string fullPath = currentPath + p[0];
                    if (currentPath[currentPath.Length - 1] != Path.DirectorySeparatorChar)
                        fullPath = currentPath + Path.DirectorySeparatorChar + p[0];
                    Encoding encoding;
                    try
                    {
                        encoding = Encoding.GetEncoding(p[1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{p[1]} encoding is not supported:\n{e.Message}");
                        return;
                    }
                    string text = string.Join(" ", p.Skip(2).ToArray());
                    try
                    {
                        File.WriteAllText(fullPath, text, encoding);
                        Console.WriteLine($"all text was written to {fullPath} in encoding {encoding}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"couldn't reach {fullPath}:\n{e.Message}");
                    }
                }


            /// <summary>
            /// Mkdir: creates new directory in current path. usage:
            /// mkdir <dirName>
            /// You should provide just directory name, not path.
            /// </summary>
            /// <param name="p"></param>
            public static void Mkdir(params string[] p)
                {
                    if (p.Length != 1)
                    {
                        NotifyHelp();
                        return;
                    }
                    string fullPath = currentPath;
                    if (currentPath[currentPath.Length - 1] != Path.DirectorySeparatorChar)
                        fullPath = currentPath + Path.DirectorySeparatorChar;
                    fullPath += p[0];
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        Console.WriteLine($"directory {fullPath} was created");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"couldn't reach {fullPath}:\n{e.Message}");
                    }
                }


            /// <summary>
            /// Merge: merging 2 or more files and writing to console. usage:
            /// merge <absoluteFullFileName>...2+
            /// There must be at least two full file names and full file name = path + fileName.
            /// </summary>
            /// <param name="p"></param>
            public static void Merge(params string[] p)
                {
                    if (p.Length < 2)
                    {
                        NotifyHelp();
                        return;
                    }
                    StringBuilder result = new StringBuilder();
                    foreach (var el in p)
                    {
                        try
                        {
                            result.Append(File.ReadAllText(el, Encoding.UTF8) + "\n");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"file {el} was unreachable:\n{e.Message}");
                            return;
                        }
                    }
                    Console.WriteLine($"result of merge:\n{result}");
                }
            }

            // Delegate used for containing command methods.
            // Delegates: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/
            // Params: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params
            private delegate void CommandDelegate(params string[] p);


            protected static string currentPath = "";

            // Encodings that could be used in commands.
            protected static HashSet<string> encodingSet = new HashSet<string>() { "utf-16", "unicodeFFFE", "utf-32", "utf-32BE",
                "us-ascii", "iso-8859-1", "utf-7", "utf-8" };



            private static Dictionary<string, CommandDelegate> commands = new Dictionary<string, CommandDelegate>()
            {
            };

            // This dictionary contains all "help" info.
            private static Dictionary<string, string> commandDescription = new Dictionary<string, string>()
            {
                ["help"] = "command that outputs info about all commands and their usage in FileManager. usage:\n" +
                "help [command]\n" +
                "where command specifies which command info are you looking for.\n" +
                "and without using it you get all commands info.",
                ["diskinfo"] = "command that prints info about your disks. usage:\n" +
                "diskinfo [diskName]...\n" +
                "where disk prints all disks' info, while specifiying disk names prints only their info in format that disks are named when using \"diskinfo\"",
                ["disk"] = "command that sets current disk to specified. usage:\n" +
                "disk <diskName>\n" +
                "where diskName is obviously disk name",
                ["ls"] = "prints files, contained within directory. usage:\n" +
                "ls [path]\n" +
                "by default shows files in current directory, but with specified path - within it",
                ["cd"] = "enters the specified directory. usage:\n" +
                "cd [directory]\n" +
                "where directory could be \"..\" to go up one level",
                ["cat"] = "command shows the file content. usage:\n" +
                "cat <fileName> [encoding]\n" +
                "where encoding is encoding in which content will be shown (default is UTF-8), and fileName is name of the file in the current path\n" +
                $"all encoding variants are: {string.Join(", ", encodingSet)}",
                ["cp"] = "method for copiying file into another directory. usage:\n" +
                "cp <fileName> <fullNameToDirectoryToCopy>\n" +
                "fileName is relative to current path and fullNameToDirectoryToCopy is absolute",
                ["mv"] = "method for moving file into another directory. usage:\n" +
                "mv <fileName> <pathToDirectoryToMove>\n" +
                "fileName is relative to current path and pathToDirectoryToMove is absolute",
                ["rm"] = "method for removing file from system. usage:\n" +
                "rm <fileName>\n" +
                "fileName is file name - this file will be deleted",
                ["touch"] = "creates file with set encoding. usage:\n" +
                "touch <fileName> <encoding> <text>...\n" +
                "you MUST set an encoding in order this method to work? for utf-8 it's \"utf-8\"\n" +
                $"all encoding variants are: {string.Join(", ", encodingSet)}",
                ["mkdir"] = "creates new directory in current path. usage:\n" +
                "mkdir <dirName>\n" +
                "you should provide just directory name, not path",
                ["merge"] = "merging 2 or more files and writing to console. usage:\n" +
                "merge <absoluteFullFileName>...2+\n" +
                "there must be at least two full file names and full file name = path + fileName"
            };


            /// <summary>
            /// Parse input.
            /// </summary>
            /// <returns> Parsed input. </returns>
            private static string[] ParseInput()
            {
                string[] input;
                do
                    input = Console.ReadLine().Split();
                while (input.Length == 0);
                input[0] = input[0].ToLower();
                List<string> inpList = new List<string>();
                foreach (var el in input)
                    if (el != "")
                        inpList.Add(el);
                inpList = inpList.Select(str => str.Replace('|', ' ')).ToList();
                return inpList.ToArray();
            }


            // Lamdas: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
            // Delegates: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/
            // In this method all commands will be mapped.


            /// <summary>
            /// Map commands to functions.
            /// </summary>
            private static void SetupCommandsDictionary()
            {
                commands.Add("help", param => CommandMethods.Help(param));
                commands.Add("diskinfo", param => CommandMethods.DiskInfo(param));
                commands.Add("disk", param => CommandMethods.Disk(param));
                commands.Add("ls", param => CommandMethods.Ls(param));
                commands.Add("cd", param => CommandMethods.Cd(param));
                commands.Add("cat", param => CommandMethods.Cat(param));
                commands.Add("cp", param => CommandMethods.Cp(param));
                commands.Add("mv", param => CommandMethods.Mv(param));
                commands.Add("rm", param => CommandMethods.Rm(param));
                commands.Add("touch", param => CommandMethods.Touch(param));
                commands.Add("merge", param => CommandMethods.Merge(param));
                commands.Add("mkdir", param => CommandMethods.Mkdir(param));
            }


            
            /// <summary>
            /// Launches intednded command.
            /// </summary>
            /// <param name="input"></param>
            private static void LaunchCommand(string[] input)
            {
                if (input.Length == 0)
                {
                    CommandMethods.NotifyHelp();
                    return;
                }
                if (commands.ContainsKey(input[0]))
                    commands[input[0]](input.Skip(1).ToArray());
                else
                    CommandMethods.NotifyHelp();
            }


            /// <summary>
            /// Setups default path.
            /// </summary>
            private static void SetupCurrentPath()
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (var d in allDrives)
                    if (d.IsReady)
                        currentPath = d.Name;
            }


            // Additional and more specified functionality are:
            // - Help contains Bacus-Naur form of queue, and can be used to provide info about specific operator.
            // - Help call is specified within each method in CommandMethods using reflection.
            // - There could be used 9 encodings instead of 3.
            // - Mkdir method
            // - Cd method works with ..
            // ------
            // Amount of comments is pretty enough because code is really simple, and commandDescription should be used as documentation for CommandMethods methods. 
            static void Main(string[] args)
            {
                CommandMethods.NotifyHelp();
                SetupCommandsDictionary();
                SetupCurrentPath();
                while (true)
                {
                    Console.WriteLine($"{currentPath}>");
                    string[] input = ParseInput();
                    Console.WriteLine();
                    LaunchCommand(input);
                }
            }
        }
    }