using System;
using System.IO;

namespace DelegatesAndEvents.FileSearch
{
    //new feature of .net core. 
    public class FileFoundArgs// : EventArgs
    {
        public string FoundFile { get; }
        public bool CancelRequested { get; set; }

        public FileFoundArgs(string fileName)
        {
            FoundFile = fileName;
        }
    }

    internal class SearchDirectoryArgs : EventArgs
    {
        internal string CurrentSearchDirectory { get; }
        internal int TotalDirs { get; }
        internal int CompleteDirs { get; }

        public SearchDirectoryArgs(string dir, int totalDirs, int completeDirs)
        {
            CurrentSearchDirectory = dir;
            TotalDirs = totalDirs;
            CompleteDirs = completeDirs;
        }
    }

    public class FileSearcher
    {
        //public async EventHandler<FileFoundArgs> HandlerAsync;

        public event EventHandler<FileFoundArgs> FileFound;

        internal event EventHandler<SearchDirectoryArgs> DirectoryChanged
        {
            add { directoryChanged += value; }
            remove { directoryChanged -= value; }
        }
        private EventHandler<SearchDirectoryArgs> directoryChanged;

        public void Search(string directory, string searchPattern)
        {
            foreach (var file in Directory.EnumerateFiles(directory, searchPattern))
            {
                FileFound?.Invoke(this, new FileFoundArgs(file));
            }
        }

        public void Search(string directory, string searchPattern, bool searchSubDirs = false)
        {
            if (searchSubDirs)
            {
                var allDirectories = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories);
                var completeDirs = 0;
                var totalDirs = allDirectories.Length + 1;
                foreach (var dir in allDirectories)
                {
                    System.Threading.Thread.Sleep(1);
                    directoryChanged?.Invoke(this,
                    new SearchDirectoryArgs(dir, totalDirs, completeDirs++));

                    //Recursively search this child directory
                    SearchDirectory(dir, searchPattern);
                }
                //Include the Current Directory
                directoryChanged?.Invoke(this,
                    new SearchDirectoryArgs(directory, totalDirs, completeDirs++));

                SearchDirectory(directory, searchPattern);
            }
            else
            {
                SearchDirectory(directory, searchPattern);
            }
        }

        private void SearchDirectory(string directory, string searchPattern)
        {
            foreach (var file in Directory.EnumerateFiles(directory, searchPattern))
            {
                var args = new FileFoundArgs(file);
                FileFound?.Invoke(this, args);
                if (args.CancelRequested) break;
            }
        }

        public static void Run()
        {
            var filesFound = 1;

            EventHandler<FileFoundArgs> onFileFound = (sender, eventArgs) =>
            {
                Console.WriteLine($"{filesFound}: {eventArgs.FoundFile}");
                filesFound++;
            };

            var fileSearcher = new FileSearcher();
            fileSearcher.FileFound += onFileFound;
            fileSearcher.DirectoryChanged += (sender, eventArgs) =>
            {
                Console.Clear();
                Console.Write($"Entering '{eventArgs.CurrentSearchDirectory}'.");
                Console.WriteLine($" {eventArgs.CompleteDirs} of {eventArgs.TotalDirs} completed...");
            };


            fileSearcher.Search("/Users/jonatanmachado/estudo-react/counter-app", "*.js", true);
        }

    }
}
