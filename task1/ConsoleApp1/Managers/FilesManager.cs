using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class FilesManager : IShow
    {
        public FileModel file = new FileModel();
        public List<string> Files { get; private set; }

        public void GetFiles(string path)
        {
            Files = Directory.GetFiles(path).ToList();
        }
        public string GetTextFromFile()
        {
            string output = File.ReadAllText(file.Path);
            return output;
        }
        public void WriteTextToFile(string text)
        {
            File.WriteAllText(file.Path, text);
        }
        public void Show()
        {
            Files.Sort();

            foreach (var file in Files)
            {
                Console.WriteLine("\n" + Path.GetFileName(file));
            }
        }
    }
}
