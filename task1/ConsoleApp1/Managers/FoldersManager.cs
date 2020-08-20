using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class FoldersManager : IShow
    {
        public FolderModel folder = new FolderModel();
        public List<string> Dirs { get; set; }

        public void GetParentDir()
        {
            int id = 0;
            string currentDir = Dirs[id].Substring(0, Dirs[id].LastIndexOf('\\'));
            folder.ParentDir = Directory.GetParent(currentDir);
        }
        public void GoNext(int id)
        {
            Dirs = Directory.GetDirectories(Dirs[id - 1]).ToList();
        }
        public void GoBack()
        {
            Dirs = Directory.GetDirectories(folder.ParentDir.FullName).ToList();
        }
        public void Show()
        {
            Dirs.Sort();
            Console.WriteLine("\n0 \t Up");
            for (int i = 0; i < Dirs.Count; i++)
            {
                Console.WriteLine(i + 1 + "\t" + Dirs[i]);
            }
        }
    }
}
