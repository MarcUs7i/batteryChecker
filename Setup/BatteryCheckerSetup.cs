using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class BatteryCheckerSetup
{
    static void Main()
    {
        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        string programName = "BatteryChecker.exe";
        string programPath = Path.Combine(startupFolder, programName);

        // Made by Marc Tismonar
        Console.WriteLine("Made by Marc Tismonar");
        Console.WriteLine(" ");

        if (File.Exists(programPath))
        {
            Console.WriteLine("BatteryChecker is already installed.");
            Console.Write("Do you want to uninstall it? (yes/no): ");
            string response = Console.ReadLine().Trim().ToLower();

            if (response == "yes")
            {
                StopAndUninstallBatteryChecker(programPath);
            }
            else
            {
                Console.WriteLine("Uninstall cancelled. BatteryChecker remains installed.");
            }
        }
        else
        {
            Console.WriteLine("BatteryChecker is not installed.");
            Console.Write("Do you want to install it? (yes/no): ");
            string response = Console.ReadLine().Trim().ToLower();

            if (response == "yes")
            {
                InstallAndStartBatteryChecker(programPath);
            }
            else
            {
                Console.WriteLine("Installation cancelled. BatteryChecker remains uninstalled");
            }
        }

        Console.WriteLine("Press any key to exit the setup.");
        Console.ReadKey();
    }

    static void StopAndUninstallBatteryChecker(string programPath)
    {
        // Check if BatteryChecker is running and stop it
        Process[] processes = Process.GetProcessesByName("BatteryChecker");
        if (processes.Length > 0)
        {
            foreach (var process in processes)
            {
                process.Kill();
                //ExecuteCommand(@"taskkill /f /im BatteryChecker.exe");
                Thread.Sleep(2);
            }
        }

        // Delete the program
        File.Delete(programPath);
        Console.WriteLine("BatteryChecker has been removed from your computer.");
    }

    static void InstallAndStartBatteryChecker(string programPath)
    {
        // Copy the program to the Startup folder
        File.Copy("BatteryChecker.exe", programPath);
        Console.WriteLine("BatteryChecker is now installed.");

        // Start the program
        try
        {
            Process.Start(programPath);
            Console.WriteLine("BatteryChecker has been started.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to start BatteryChecker: " + ex.Message);
        }
    }

    static void ExecuteCommand(string command)
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C {command}";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Command failed. Output: {output}\nError: {error}");
            }
        }
    }
}
