using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryObserver
{
    internal class Program
    {
        private static HashSet<string> oldPaths = new HashSet<string>();
        private static HashSet<string> newPaths = new HashSet<string>();

        [STAThread]
        private static void Main(string[] args)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("Folder not found");
                Console.ReadKey();
                return;
            }
            do
            {
                IterateDirectories(fbd.SelectedPath);
                CompareSets();
                Console.WriteLine("\n\nPress any key to Scan again!");
                Console.ReadKey();
                Console.Clear();
            } while (true);
        }

        //Recursive Iteration
        private static void IterateDirectories(string path)
        {
            //Read all Directories and Files of current directory
            var dir = Directory.EnumerateDirectories(path);
            var files = Directory.EnumerateFiles(path);

            //Add all paths to Set and all new paths to newlist
            foreach (var p in dir)
            {
                newPaths.Add(p);
            }
            foreach (var p in files)
            {
                newPaths.Add(p);
            }

            //Iterate and execute recursion
            foreach (var d in dir)
            {
                IterateDirectories(d);
            }
        }

        private static void CompareSets()
        {
            var deleted = oldPaths.Except(newPaths).ToList();
            var added = newPaths.Except(oldPaths).ToList();
            Console.WriteLine($"{deleted.Count + added.Count} Changes detected!\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            foreach (var p in deleted)
            {
                Console.WriteLine($"File deleted: {p}");
                oldPaths.Remove(p);
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            foreach (var p in added)
            {
                Console.WriteLine($"File added: {p}");
                oldPaths.Add(p);
            }
            Console.ResetColor();
            newPaths.Clear();
        }
    }
}