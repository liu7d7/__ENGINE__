using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace __ENGINE__
{
    public static class Program
    {
        private const string engine = "__ENGINE__";
        private static string _projectName = "";
        private static string _pathToProject = "";
        
        // ReSharper disable once InconsistentNaming
        [STAThread]
        public static void Main(string[] args)
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Console.WriteLine("What's the name of this project? ");
            _projectName = Console.ReadLine();
            _pathToProject = $"{userFolder}\\RiderProjects\\{_projectName}";
            Program.copy_files(new DirectoryInfo($"{userFolder}\\RiderProjects\\__ENGINE__"), 
                new DirectoryInfo(_pathToProject));
            Program.replace_engine_occurrences(_pathToProject);
        }

        private static void replace_engine_occurrences(string dir)
        {
            foreach (string fileName in Directory.GetFiles(dir))
            {
                if (!fileName.EndsWith("cs") && 
                    !fileName.EndsWith("json") && 
                    !fileName.EndsWith("csproj") && 
                    !fileName.EndsWith("bat") && 
                    !fileName.EndsWith("sln") && 
                    !fileName.EndsWith("user")) continue;
                string fileContent = File.ReadAllText(fileName);
                fileContent = fileContent.Replace("__ENGINE__Main", "Main");
                fileContent = fileContent.Replace(engine, _projectName);
                File.WriteAllText(fileName, fileContent);
                Console.WriteLine($"Replaced __ENGINE__ with {_projectName} in {fileName}");
            }
            
            foreach (string directory in Directory.GetDirectories(dir))
            {
                replace_engine_occurrences(directory);
            }
        }

        private static void copy_files(DirectoryInfo source, DirectoryInfo target) {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                if (dir.FullName.Contains("net6.0") && dir.FullName.Contains("runtimes")) continue;
                
                if (dir.Name == "obj" || dir.Name.StartsWith("."))
                {
                    continue;
                }
                copy_files(dir, target.CreateSubdirectory(dir.Name.Replace(engine, _projectName)));
            }

            foreach (FileInfo file in source.GetFiles())
            {
                if (file.FullName.Contains("net6.0") && !file.FullName.Contains("Resource"))
                {
                    continue;
                }

                switch (file.Name)
                {
                    case "Program.cs":
                        continue;
                    case "__ENGINE__Program.cs":
                        file.CopyTo(Path.Combine(target.FullName, "Program.cs"));
                        continue;
                    default:
                        file.CopyTo(Path.Combine(target.FullName, file.Name.Replace(engine, _projectName)));
                        Console.WriteLine($"Copied {target.FullName}\\{file.Name} -> {target.FullName}\\{file.Name.Replace(engine, _projectName)}");
                        break;
                }
            }
        }
    }
}