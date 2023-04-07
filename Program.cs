/*
 *   Copyright (c) 2023 Az Foxxo
 *   All rights reserved.

 *   Permission is hereby granted, free of charge, to any person obtaining a copy
 *   of this software and associated documentation files (the "Software"), to deal
 *   in the Software without restriction, including without limitation the rights
 *   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *   copies of the Software, and to permit persons to whom the Software is
 *   furnished to do so, subject to the following conditions:
 
 *   The above copyright notice and this permission notice shall be included in all
 *   copies or substantial portions of the Software.
 
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *   SOFTWARE.
 */

using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ProjectIt
{
    class Program
    {
        /// <summary> Execute a command in the terminal </summary>
        public static void ExecuteCommand(string command)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "sh",
                Arguments = (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "/C " : "-c \"") + command + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "" : "\""),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            });

            process.WaitForExit();
        }

        /// <summary> Open and deserialise the JSON </summary>
        public static Root OpenJSON()
        {
            // Get the path to the executable
            string executablePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executablePath);

            // Read supported.json to get supported languages/frameworks
            string json = File.ReadAllText(executableDirectory + "/supported.json");

            // Deserialize the json into a Root object
            Root root = JsonConvert.DeserializeObject<Root>(json);

            // Return the Root object
            return root;
        }

        /// <summary> Return new project command for the specified language/framework </summary>
        public static string GetNewProjectCommand(string language, string template)
        {
            // Open and deserialise the JSON
            Root root = OpenJSON();

            // Loop through the programming languages
            foreach (ProgrammingLanguage programmingLanguage in root.programming_languages)
            {
                // Check if the language is the one we're looking for
                if (programmingLanguage.name == language)
                {
                    // Loop through the supported templates
                    foreach (List<string> projectType in programmingLanguage.package_manager.project_types)
                    {
                        // Check if the template is the one we're looking for
                        if (projectType[0] == template)
                        {
                            // If so, return the new project command
                            return projectType[1];
                        }
                    }
                }
            }

            // If the language is not found, return null
            return null;
        }

        /// <summary> Return all supported templates in a language/framework </summary>
        public static string[] GetTemplates(string language)
        {
            // Open and deserialise the JSON
            Root root = OpenJSON();

            // Loop through the programming languages
            foreach (ProgrammingLanguage programmingLanguage in root.programming_languages)
            {
                // Check if the language is the one we're looking for
                if (programmingLanguage.name == language)
                {
                    // Create an array of the supported templates
                    string[] templates = new string[programmingLanguage.package_manager.project_types.Count];

                    // Add the supported templates to the array
                    for (int i = 0; i < programmingLanguage.package_manager.project_types.Count; i++)
                    {
                        templates[i] = programmingLanguage.package_manager.project_types[i][0];
                    }

                    // Return the array
                    return templates;
                }
            }

            // If the language is not found, return null
            return null;
        }


        /// <summary> Return an array of the supported languages/frameworks </summary>
        public static string[] GetSupportedLanguages()
        {
            // Open and deserialise the JSON
            Root root = OpenJSON();

            // Create an array of the supported languages/frameworks
            string[] supportedLanguages = new string[root.programming_languages.Count];

            // Add the supported languages/frameworks to the array
            for (int i = 0; i < root.programming_languages.Count; i++)
            {
                supportedLanguages[i] = root.programming_languages[i].name;
            }

            // Return the array
            return supportedLanguages;
        }

        /// <summary> Return the package manager for the specified language/framework </summary>
        public static string GetPackageManager(string language)
        {
            // Open and deserialise the JSON
            Root root = OpenJSON();

            // Loop through the programming languages
            foreach (ProgrammingLanguage programmingLanguage in root.programming_languages)
            {
                // Check if the language is the one we're looking for
                if (programmingLanguage.name == language)
                {
                    // If so, return the package manager
                    return programmingLanguage.package_manager.name;
                }
            }

            // If the language is not found, return null
            return null;
        }


        /// <summary> Return the build command for the specified language/framework </summary>
        public static string GetBuildCommand(string language)
        {
            // Open and deserialise the JSON
            Root root = OpenJSON();

            // Loop through the programming languages
            foreach (ProgrammingLanguage programmingLanguage in root.programming_languages)
            {
                // Check if the language is the one we're looking for
                if (programmingLanguage.name == language)
                {
                    // If so, return the build command
                    return programmingLanguage.package_manager.build;
                }
            }

            // If the language is not found, return null
            return null;
        }

        /// <summary> Return the build release command for the specified language/framework </summary>
        public static string GetBuildReleaseCommand(string language)
        {
            // Open and deserialise the JSON
            Root root = OpenJSON();

            // Loop through the programming languages
            foreach (ProgrammingLanguage programmingLanguage in root.programming_languages)
            {
                // Check if the language is the one we're looking for
                if (programmingLanguage.name == language)
                {
                    // If so, return the build release command
                    return programmingLanguage.package_manager.build_release;
                }
            }

            // If the language is not found, return null
            return null;
        }


        /// <summary> Return the run command for the specified language/framework</summary>
        public static string GetRunCommand(string language)
        {
            // Open and deserialise the JSON
            Root root = OpenJSON();

            // Loop through the programming languages
            foreach (ProgrammingLanguage programmingLanguage in root.programming_languages)
            {
                // Check if the language is the one we're looking for
                if (programmingLanguage.name == language)
                {
                    // If so, return the run command
                    return programmingLanguage.package_manager.run;
                }
            }

            // If the language is not found, return null
            return null;
        }


        static void Main(string[] args)
        {
            // Set the directory to the directory the command is run in (not the directory the executable is in)
            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());

            // Store the arguments in a variable
            string[] arguments = args;

            // Check arguments is one or more
            if (arguments.Length < 1)
            {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run with help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Determine which action based on the first arg (run, create, help)
            switch (arguments[0])
            {
                case "run":
                    // Run the project
                    RunProject(arguments);
                    break;
                case "create":
                    // Create a new project
                    CreateProject(arguments);
                    break;
                case "build":
                    // Build the project
                    BuildProjectDev(arguments);
                    break;
                case "build-release":
                    // Build the project in release mode
                    BuildProjectRelease(arguments);
                    break;
                case "list":
                    // List languages/frameworks and projects
                    ListAllTemplatesAndLanguages();
                    break;
                case "open":
                    // Open the project
                    OpenProject(arguments);
                    break;
                case "delete":
                    // Delete the project
                    DeleteProject(arguments);
                    break;
                case "add-projectit-file":
                    // Add .projectit file to project to existing project
                    AddProjectItFile(arguments);
                    break;
                case "help":
                    // Print help
                    PrintHelp();
                    break;
                default:
                    // If the first argument is not one of the above, print an error message
                    Console.WriteLine("Error: Invalid argument! Run with --help for more information.");
                    // And exit the program
                    Environment.Exit(0);
                    break;
            }
        }

        private static void AddProjectItFile(string[] arguments)
        {
            // Check argument is provided for name and language/framework
            if (arguments.Length < 3)
            {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if the project exists
            if (!Directory.Exists(arguments[1]))
            {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist!");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if the project already has a .projectit file
            if (File.Exists($"{arguments[1]}/.projectit"))
            {
                // If so, print an error message
                Console.WriteLine("Error: Project already has a .projectit file!");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if language/framework is valid (split(":)[0] is the language, split(":")[1] is the framework)
            if (!GetSupportedLanguages().Contains(arguments[2].Split(":")[0]) || !GetTemplates(arguments[2].Split(":")[0]).Contains(arguments[2].Split(":")[1]))
            {
                // If not, print an error message
                Console.WriteLine("Error: Invalid language/framework!");
                // And exit the program
                Environment.Exit(1);
            }

            // Write the language/framework to the .projectit file
            File.WriteAllText($"{arguments[1]}/.projectit", arguments[2]);

            // Print success message
            Console.WriteLine("Successfully added .projectit file to project!");
        }

        private static void DeleteProject(string[] arguments)
        {
            // Check if there are two arguments
            if (arguments.Length < 2)
            {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if the project exists
            if (!Directory.Exists(arguments[1]))
            {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist!");
                // And exit the program
                Environment.Exit(1);
            }

            // Delete the project
            try
            {
                Directory.Delete(arguments[1], true);
                Console.WriteLine("Project deleted successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Could not delete project!");
                Console.WriteLine(e);
                Environment.Exit(1);
            }
        }

        private static void OpenProject(string[] arguments)
        {
            // Check if there are two arguments
            if (arguments.Length < 2)
            {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if the project exists
            if (!Directory.Exists(arguments[1]))
            {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist!");
                // And exit the program
                Environment.Exit(1);
            }

            // If the project exists, but no .projectit file exists, print an error message
            if (!File.Exists($"{arguments[1]}/.projectit"))
            {
                Console.WriteLine("Error: Project does not have a .projectit file!");
                // And exit the program
                Environment.Exit(1);
            }

            // Open project in VS Code
            ExecuteCommand($"code {arguments[1]}");
        }

        private static void ListAllTemplatesAndLanguages()
        {
            // List all templates and languages
            Console.WriteLine("Supported Languages (and Frameworks):");
            foreach (var programmingLanguage in GetSupportedLanguages())
            {
                // For each programming language, get it's project types
                foreach (var projectType in GetTemplates(programmingLanguage))
                {
                    // And print them
                    Console.WriteLine($"-\t{programmingLanguage}:{projectType}");
                }
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("ProjectIt Help");
            Console.WriteLine("==============");
            Console.WriteLine("Usage: projectit [command] [arguments]");
            Console.WriteLine("Commands:");
            Console.WriteLine("-\trun [project]");
            Console.WriteLine("-\tcreate [language/framework] [project]");
            Console.WriteLine("-\tbuild [project]");
            Console.WriteLine("-\tbuild-release [project]");
            Console.WriteLine("-\tlist");
            Console.WriteLine("-\topen [project]");
            Console.WriteLine("-\tdelete [project]");
            Console.WriteLine("-\tadd-projectit-file [project] [language/framework]");
        }

        private static void CreateProject(string[] arguments)
        {
            // Check if there are three arguments
            if (arguments.Length < 3)
            {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Store the language/framework and project name in variables
            string target = arguments[1];

            // Store the project name in a variable
            string project = arguments[2];

            var command = "";

            // Retrieve the command to create the project
            foreach (var programmingLanguage in GetSupportedLanguages())
            {
                // For each programming language, get it's project types
                foreach (var projectType in GetTemplates(programmingLanguage))
                {
                    // If the language/framework is supported, store the command
                    if (target == $"{programmingLanguage}:{projectType}")
                    {
                        command = GetNewProjectCommand(programmingLanguage, projectType);
                    }
                }
            }

            // If the language/framework is not supported, print an error message
            if (command == "" || command == null)
            {
                Console.WriteLine("Error: Language/framework not supported! Run list for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // If folder or directory already exists, print an error message
            if (Directory.Exists(project) || File.Exists(project))
            {
                Console.WriteLine("Error: Conflict! A folder or directory with that name already exists.");
                // And exit the program
                Environment.Exit(1);
            }

            // Create project
            command = command.Replace("{required_name}", project);
            ExecuteCommand(command);

            // Check if the directory exists
            if (!Directory.Exists(project))
            {
                // If not, print an error message
                Console.WriteLine("Error: Project could not be created!");
                // And exit the program
                Environment.Exit(1);
            }

            // Create .projectit file in project directory
            CreateProjectitFile(project, target);
        }

        private static void CreateProjectitFile(string project, string target)
        {
            // Create a file called .projectit in the project directory
            File.WriteAllText($"{project}/.projectit", target);
        }

        private static void RunProject(string[] arguments)
        {
            // Check if argument for project is provided
            if (arguments.Length < 0)
            {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Store the project name in a variable
            string project = arguments[1];

            // Check if the project exists by checking if the .projectit file exists
            if (!File.Exists($"{project}/.projectit"))
            {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Read the .projectit file
            string language = File.ReadAllText($"{project}/.projectit").Split(':')[0];

            // Change directory to project
            Console.WriteLine($"Changing directory to {project}");

            // Run project
            ExecuteCommand($"cd {project} && {GetRunCommand(language)}");

            // Exit the program
            Environment.Exit(0);
        }

        private static void BuildProjectDev(string[] arguments)
        {
            // Check if argument for project is provided
            if (arguments.Length < 0)
            {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Store the project name in a variable
            string project = arguments[1];

            // Check if the project exists by checking if the .projectit file exists
            if (!File.Exists($"{project}/.projectit"))
            {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Read the .projectit file
            string language = File.ReadAllText($"{project}/.projectit").Split(':')[0];

            // Change directory to project
            Console.WriteLine($"Changing directory to {project}");

            // Build project
            ExecuteCommand($"cd {project} && {GetBuildCommand(language)}");

            // Exit the program
            Environment.Exit(0);
        }

        private static void BuildProjectRelease(string[] arguments)
        {
            // Check if argument for project is provided
            if (arguments.Length < 0)
            {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Store the project name in a variable
            string project = arguments[1];

            // Check if the project exists by checking if the .projectit file exists
            if (!File.Exists($"{project}/.projectit"))
            {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Read the .projectit file
            string language = File.ReadAllText($"{project}/.projectit").Split(':')[0];

            // Change directory to project
            Console.WriteLine($"Changing directory to {project}");

            // Build project
            ExecuteCommand($"cd {project} && {GetBuildReleaseCommand(language)}");

            // Exit the program
            Environment.Exit(0);
        }
    }
}