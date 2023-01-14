/*
 *   Copyright (c) 2023 Az Foxxo
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

namespace ProjectIt {
    class Program {
        static void Main(string[] args) {
            // Set the directory to the directory the command is run in (not the directory the executable is in)
            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory());

            // Store the arguments in a variable
            string[] arguments = args;

            // Check arguments is one or more
            if (arguments.Length < 1) {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run with help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Determine which action based on the first arg (run, create, help)
            switch (arguments[0]) {
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
                    BuildProject(arguments);
                    break;
                case "list":
                    // List languages/frameworks and projects
                    ListProjects();
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
            if (arguments.Length < 3) {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if the project exists
            if (!Directory.Exists(arguments[1])) {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist!");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if the project already has a .projectit file
            if (File.Exists($"{arguments[1]}/.projectit")) {
                // If so, print an error message
                Console.WriteLine("Error: Project already has a .projectit file!");
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
            if (arguments.Length < 2) {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if the project exists
            if (!Directory.Exists(arguments[1])) {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist!");
                // And exit the program
                Environment.Exit(1);
            }

            // Delete the project
            try {
                Directory.Delete(arguments[1], true);
                Console.WriteLine("Project deleted successfully!");
            } catch (Exception e) {
                Console.WriteLine("Error: Could not delete project!");
                Console.WriteLine(e);
                Environment.Exit(1);
            }
        }

        private static void OpenProject(string[] arguments)
        {
            // Check if there are two arguments
            if (arguments.Length < 2) {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Check if the project exists
            if (!Directory.Exists(arguments[1])) {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist!");
                // And exit the program
                Environment.Exit(1);
            }

            // If the project exists, but no .projectit file exists, print an error message
            if (!File.Exists($"{arguments[1]}/.projectit")) {
                Console.WriteLine("Error: Project does not have a .projectit file!");
                // And exit the program
                Environment.Exit(1);
            }

            // Open project in VS Code
            Process.Start("code", arguments[1]);
        }

        private static void ListProjects()
        {
            // List all supported languages/frameworks
            Console.WriteLine("Supported languages/frameworks:");
            Console.WriteLine("  C#:console");
            Console.WriteLine("  C#:classlib");
            Console.WriteLine("  Rust:bin");
            Console.WriteLine("  Rust:lib");
            Console.WriteLine("  Dart:console");
            Console.WriteLine("  Dart:lib");
            Console.WriteLine("  Dart:flutter");    
            Console.WriteLine("  Dart:lib-flutter");
            Console.WriteLine("  MonoGame:C#");
        }

        private static void PrintHelp()
        {
            Console.WriteLine("ProjectIt Help");
            Console.WriteLine("==============");
            Console.WriteLine("Usage: projectit [command] [arguments]");
            Console.WriteLine("Commands:");
            Console.WriteLine("  run [project]");
            Console.WriteLine("  create [language/framework] [project]");
            Console.WriteLine("  build [project]");
            Console.WriteLine("  list");
            Console.WriteLine("  open [project]");
            Console.WriteLine("  delete [project]");
            Console.WriteLine("  add-projectit-file [project] [language/framework]");
        }

        private static void CreateProject(string[] arguments)
        {
            // Check if there are three arguments
            if (arguments.Length < 3) {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Store the language/framework and project name in variables
            string language = arguments[1];
            
            // Store the project name in a variable
            string project = arguments[2];

            var command = "";

            // Determine which language/framework to use

            // C#
            if (language == "C#:console") command = $"dotnet new console -o {project}";
            if (language == "C#:classlib") command = $"dotnet new classlib -o {project}";
            
            // Rust
            if (language == "Rust:bin") command = $"cargo new {project}";
            if (language == "Rust:lib") command = $"cargo new {project} --lib";

            // Dart
            if (language == "Dart:console") command = $"dart create {project}";
            if (language == "Dart:lib") command = $"dart create -t lib {project}";

            // Flutter
            if (language == "Dart:flutter") command = $"flutter create {project}";
            if (language == "Dart:lib-flutter") command = $"flutter create -t lib {project}";

            // MonoGame
            if (language == "MonoGame:C#") command = $"dotnet new mgdesktopgl -o {project}";

            // If the language/framework is not supported, print an error message
            if (command == "") {
                Console.WriteLine("Error: Language/framework not supported! Run list for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // If folder or directory already exists, print an error message
            if (Directory.Exists(project) || File.Exists(project)) {
                Console.WriteLine("Error: Conflict! A folder or directory with that name already exists.");
                // And exit the program
                Environment.Exit(1);
            }

            // Create project (linux and mac and windows)
            var process = new Process();
            process.StartInfo.FileName = "sh";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = $"-c \"{command}\"";
            process.Start();
            process.WaitForExit();

            // Open project in VS Code
            var process2 = new Process();
            process2.StartInfo.FileName = "sh";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) process2.StartInfo.FileName = "cmd";
            process2.StartInfo.Arguments = $"-c \"code {project}\"";
            process2.Start();
            process2.WaitForExit();

            // Create a file in the project directory called .projectit
            File.WriteAllText($"{project}/.projectit", language);
        }

        private static void RunProject(string[] arguments)
        {
            // Check if argument for project is provided
            if (arguments.Length < 2) {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(0);
            }

            // Store the project name in a variable
            string project = arguments[1];

            // Check if the project exists by checking if the .projectit file exists
            if (!File.Exists($"{project}/.projectit")) {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist! Run help for more information.");
                // And exit the program
                Environment.Exit(0);
            }

            // Read the .projectit file
            string language = File.ReadAllText($"{project}/.projectit");

            // Determine which language/framework to use
            var command = "";

            // C#
            if (language == "C#:console") command = $"cd {project} && dotnet run";
            if (language == "C#:classlib") { Console.WriteLine("Error: Cannot run a classlib project!"); Environment.Exit(0); }

            // Rust
            if (language == "Rust:bin") command = $"cd {project} && cargo run";
            if (language == "Rust:lib") { Console.WriteLine("Error: Cannot run a library project!"); Environment.Exit(0); }

            // Dart
            if (language == "Dart:console") command = $"cd {project} && dart run";
            if (language == "Dart:lib") { Console.WriteLine("Error: Cannot run a library project!"); Environment.Exit(0); }

            // Flutter
            if (language == "Dart:flutter") command = $"cd {project} && flutter run";
            if (language == "Dart:lib-flutter") { Console.WriteLine("Error: Cannot run a library project!"); Environment.Exit(0); }

            // MonoGame
            if (language == "MonoGame:C#") command = $"cd {project} && dotnet run";

            // Run project (linux and mac and windows)
            var process = new Process();
            process.StartInfo.FileName = "sh";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = $"-c \"{command}\"";
            process.Start();
            process.WaitForExit();

            // Exit the program
            Environment.Exit(0);
        }

        private static void BuildProject(string[] arguments) {
            // Check if argument for project is provided
            if (arguments.Length < 2) {
                // If not, print an error message
                Console.WriteLine("Error: Not enough arguments! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Store the project name in a variable
            string project = arguments[1];

            // Check if the project exists by checking if the .projectit file exists
            if (!File.Exists($"{project}/.projectit")) {
                // If not, print an error message
                Console.WriteLine("Error: Project does not exist! Run help for more information.");
                // And exit the program
                Environment.Exit(1);
            }

            // Read the .projectit file
            string language = File.ReadAllText($"{project}/.projectit");

            // Determine which language/framework to use
            var command = "";

            // C#
            if (language == "C#:console") command = $"cd {project} && dotnet build";
            if (language == "C#:classlib") command = $"cd {project} && dotnet build";

            // Rust
            if (language == "Rust:bin") command = $"cd {project} && cargo build";
            if (language == "Rust:lib") command = $"cd {project} && cargo build";

            // Dart
            if (language == "Dart:console") command = $"cd {project} && dart compile exe bin/main.dart";
            if (language == "Dart:lib") command = $"cd {project} && dart compile exe bin/main.dart";

            // Flutter
            if (language == "Dart:flutter") command = $"cd {project} && flutter build";
            if (language == "Dart:lib-flutter") command = $"cd {project} && flutter build";

            // MonoGame
            if (language == "MonoGame:C#") command = $"cd {project} && dotnet build";

            // Build project (linux and mac and windows)
            var process = new Process();
            process.StartInfo.FileName = "sh";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = $"-c \"{command}\"";
            process.Start();
            process.WaitForExit();

            // Exit the program
            Environment.Exit(0);
        }
    }
}