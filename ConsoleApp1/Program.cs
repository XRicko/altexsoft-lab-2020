using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Decide what you wanna do: \n 1 - work with text file \n 2 - browse through folders \n 3 - exit ");
                string mode = Console.ReadLine();

                string text = "";
                string path;

                switch (mode)
                {
                    case "1":
                        Console.Clear();

                        try
                        {
                            do
                            {
                                Console.Write("Type path to the file: ");
                                path = Console.ReadLine();
                                if (!File.Exists(path))
                                    Console.WriteLine("Invalid path\n");
                                else
                                {
                                    text = File.ReadAllText(path);
                                }
                            } while (!File.Exists(path));

                            Console.Clear();

                            Console.WriteLine("Choose option: \n 1 - remove symbols in text \n 2 - number of words in text and see every 10th word \n 3 - 3rd sentence backwards");
                            string option = Console.ReadLine();

                            if (option == "1")
                            {
                                if (!File.Exists("my.cop"))
                                    File.Copy(path, "my.cop");

                                Console.Write("Enter symbol(s) you want to remove: ");
                                string symbols = Console.ReadLine();

                                if (TextManager.RemoveSymbols(text, symbols) != null)
                                {
                                    string replaced = TextManager.RemoveSymbols(text, symbols);
                                    File.WriteAllText(path, replaced);
                                }
                                else
                                    Console.WriteLine("No such symbol(s) in the text\n");
                            }
                            else if (option == "2")
                            {
                                string noPunct = TextManager.DeletePunctuation(text);
                                string[] words = noPunct.Split(new string[] { " ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                                Console.WriteLine("Words in text: " + words.Length + "\n");

                                int nthWord = 10;
                                List<string> nWords = TextManager.GetEveryNthWord(words, nthWord);

                                if (nthWord.ToString().EndsWith("2"))
                                    Console.Write($"Every {nthWord}nd word: ");
                                else if (nthWord.ToString().EndsWith("3"))
                                    Console.Write($"Every {nthWord}rd word: ");
                                else
                                    Console.Write($"Every {nthWord}th word: ");

                                for (int i = 0; i < nWords.Count; i++)
                                {
                                    if (i == nWords.Count - 1)
                                        Console.WriteLine(nWords[i] + "\n");
                                    else
                                        Console.Write(nWords[i] + ", ");
                                }
                            }
                            else if (option == "3")
                            {
                                string[] sentences = text.Split(new char[] { '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
                                int nthSentence = 3;
                                Console.WriteLine($"{nthSentence} sentence backwards: " + TextManager.ReverseString(sentences[nthSentence - 1]) + "\n");
                            }
                            else
                                Console.WriteLine("Invalid input\n");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "\n");
                        }

                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("Type path to the folder: ");

                        try
                        {
                            path = Console.ReadLine();
                            List<string> dirs = Directory.GetDirectories(path).ToList();
                            string browseMode = "";
                            int id = 0;

                            do
                            {
                                if (dirs.Count == 0)
                                    Console.WriteLine("No folders here\n");
                                else
                                {
                                    dirs.Sort();
                                    ShowDirs(dirs);

                                    Console.Write("Choose folder to see files/directories in it (e.g. 1f - for files, 1d - for directories): ");
                                    browseMode = Console.ReadLine();
                                    id = Convert.ToInt32(browseMode.Remove(browseMode.Length - 1));

                                    path = dirs[id];
                                    dirs = Directory.GetDirectories(path).ToList();
                                }
                            } while (browseMode.EndsWith("d"));

                            if (browseMode.EndsWith("f"))
                            {
                                List<string> files = Directory.GetFiles(dirs[id]).ToList();

                                if (files.Count == 0)
                                    Console.WriteLine("No files here\n");
                                else
                                {
                                    files.Sort();

                                    foreach (var file in files)
                                    {
                                        Console.WriteLine(Path.GetFileName(file));
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "\n");
                        }

                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid input\n");
                        break;
                }
            }
        }
        static void ShowDirs(List<string> dirs)
        {
            for (int i = 0; i < dirs.Count; i++)
            {
                Console.WriteLine(i + "\t" + dirs[i]);
            }
        }
    }
}
