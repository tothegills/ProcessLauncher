using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ProcessLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var executable = GetExecutable(args);
            var arguments = GetArguments(args);

            var exitCode = Launch(executable, arguments, currentDirectory);
            Console.WriteLine($"Exit code: {exitCode}");
        }

        static string GetExecutable(IReadOnlyList<string> args)
        {
            if (args.Any())
                return args[0];

            Console.Write("Command to execute: ");
            return Console.ReadLine();
        }

        static string GetArguments(string[] args)
        {
            if (args.Any())
                return string.Join(" ", args.Skip(1));

            Console.Write("Arguments: ");
            return Console.ReadLine();
        }

        static int Launch(string executable, string arguments, string workingDirectory)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = executable,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            return process.ExitCode;
        }
    }
}
