using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace InstaShit.CLI.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("InstaShit.CLI.Publisher");
            Console.WriteLine("Created by Konrad Krawiec");
            Console.WriteLine("THIS TOOL IS NOT INTENDED FOR PUBLIC USAGE\n");
            if(args.Length != 2)
            {
                Console.WriteLine("Usage: InstaShit.CLI.Publisher <path> <version>");
                return;
            }
            string assemblyLocation = args[0];
            string version = args[1];
            string basePath = Path.Combine(assemblyLocation, "bin", "Release",
                                           "netcoreapp2.0");
            string pwd = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string movePath = Path.Combine(pwd, "tmp");
            if (File.Exists(Path.Combine(assemblyLocation, "InstaShit.sln")))
                assemblyLocation = Path.Combine(assemblyLocation, "InstaShit");
            else if (!File.Exists(Path.Combine(assemblyLocation, "InstaShit.csproj")))
            {
                Console.WriteLine("[ERROR] The given path is not InstaShit.CLI directory.");
                return;
            }
            if (!Directory.Exists(Path.Combine(pwd, "Publisher")))
                Directory.CreateDirectory(Path.Combine(pwd, "Publisher"));
            if(Directory.Exists(movePath))
            {
                Console.WriteLine("Cleaning temporary files");
                Directory.Delete(movePath, true);
            }
            Console.WriteLine("Cleaning output files");
            Process.Start("dotnet", $"clean {assemblyLocation}").WaitForExit();
            Console.WriteLine("Creating portable release");
            Process.Start("dotnet", $"publish -c Release {assemblyLocation}").WaitForExit();
            Console.WriteLine("Creating win-x86 release");
            Process.Start("dotnet", $"publish -c Release -r win-x86 {assemblyLocation}").WaitForExit();
            Console.WriteLine("Creating win-x64 release");
            Process.Start("dotnet", $"publish -c Release -r win-x64 {assemblyLocation}").WaitForExit();
            Console.WriteLine("Creating win-arm release");
            Process.Start("dotnet", $"publish -c Release -r win-arm {assemblyLocation}").WaitForExit();
            Console.WriteLine("Creating linux-x64 release");
            Process.Start("dotnet", $"publish -c Release -r linux-x64 {assemblyLocation}").WaitForExit();
            Console.WriteLine("Creating linux-arm release");
            Process.Start("dotnet", $"publish -c Release -r linux-arm {assemblyLocation}").WaitForExit();
            Console.WriteLine("Creating osx-x64 release");
            Process.Start("dotnet", $"publish -c Release -r osx-x64 {assemblyLocation}").WaitForExit();
            Directory.CreateDirectory(movePath);
            System.Threading.Thread.Sleep(500);
            Console.WriteLine("Renaming all \"publish\" directories to \"InstaShit\"");
            Directory.CreateDirectory(Path.Combine(movePath, "win-x86"));
            Directory.CreateDirectory(Path.Combine(movePath, "win-x64"));
            Directory.CreateDirectory(Path.Combine(movePath, "win-arm"));
            Directory.CreateDirectory(Path.Combine(movePath, "linux-x64"));
            Directory.CreateDirectory(Path.Combine(movePath, "linux-arm"));
            Directory.CreateDirectory(Path.Combine(movePath, "osx-x64"));
            Directory.Move(Path.Combine(basePath, "publish"),
                           Path.Combine(movePath, "InstaShit"));
            Directory.Move(Path.Combine(basePath, "win-x86", "publish"),
                           Path.Combine(movePath, "win-x86", "InstaShit"));
            Directory.Move(Path.Combine(basePath, "win-x64", "publish"),
                           Path.Combine(movePath, "win-x64", "InstaShit"));
            Directory.Move(Path.Combine(basePath, "win-arm", "publish"),
                           Path.Combine(movePath, "win-arm", "InstaShit"));
            Directory.Move(Path.Combine(basePath, "linux-x64", "publish"),
                           Path.Combine(movePath, "linux-x64", "InstaShit"));
            Directory.Move(Path.Combine(basePath, "linux-arm", "publish"),
                           Path.Combine(movePath, "linux-arm", "InstaShit"));
            Directory.Move(Path.Combine(basePath, "osx-x64", "publish"),
                           Path.Combine(movePath, "osx-x64", "InstaShit"));
            Console.WriteLine("Zipping portable release");
            ZipFile.CreateFromDirectory(Path.Combine(movePath, "InstaShit"),
                                        Path.Combine(pwd, "Publisher", $"InstaShit-v{version}.zip"));
            Console.WriteLine("Zipping win-x86 release");
            ZipFile.CreateFromDirectory(Path.Combine(movePath, "win-x86", "InstaShit"),
                                        Path.Combine(pwd, "Publisher", $"InstaShit-v{version}-win-x86.zip"));
            Console.WriteLine("Zipping win-x64 release");
            ZipFile.CreateFromDirectory(Path.Combine(movePath, "win-x64", "InstaShit"),
                                        Path.Combine(pwd, "Publisher", $"InstaShit-v{version}-win-x64.zip"));
            Console.WriteLine("Zipping win-arm release");
            ZipFile.CreateFromDirectory(Path.Combine(movePath, "win-arm", "InstaShit"),
                                        Path.Combine(pwd, "Publisher", $"InstaShit-v{version}-win-arm.zip"));
            Console.WriteLine("Zipping linux-x64 release");
            ZipFile.CreateFromDirectory(Path.Combine(movePath, "linux-x64", "InstaShit"),
                                        Path.Combine(pwd, "Publisher", $"InstaShit-v{version}-linux-x64.zip"));
            Console.WriteLine("Zipping linux-arm release");
            ZipFile.CreateFromDirectory(Path.Combine(movePath, "linux-arm", "InstaShit"),
                                        Path.Combine(pwd, "Publisher", $"InstaShit-v{version}-linux-arm.zip"));
            Console.WriteLine("Zipping osx-x64 release");
            ZipFile.CreateFromDirectory(Path.Combine(movePath, "osx-x64", "InstaShit"),
                                        Path.Combine(pwd, "Publisher", $"InstaShit-v{version}-osx-x64.zip"));
            Console.WriteLine("Cleaning up");
            Directory.Delete(movePath, true);
            Console.WriteLine("All done!");
        }
    }
}
