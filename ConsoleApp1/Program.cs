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
                string path;

                switch (mode)
                {
                    case "1":
                        Console.Clear();

                        try
                        {
                            string text;

                            while (true)
                            {
                                Console.Write("Type path to the file: ");
                                path = Console.ReadLine();

                                if (!File.Exists(path))
                                    Console.WriteLine("\nInvalid path\n");
                                else
                                {
                                    text = File.ReadAllText(path);
                                    break;
                                }
                            }

                            Console.Clear();
                            Console.WriteLine("Choose an option: \n 1 - remove symbols in text \n 2 - number of words in text and see every 10th word \n 3 - 3rd sentence backwards");

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
                                    Console.WriteLine("\nNo such symbol(s) in the text\n");
                            }
                            else if (option == "2")
                            {
                                string noPunct = TextManager.DeletePunctuation(text);
                                string[] words = noPunct.Split(new string[] { " ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                                Console.WriteLine("\nWords in text: " + words.Length + "\n");

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
                                Console.WriteLine("\nInvalid input\n");
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

                            while (true)
                            {
                                if (dirs.Count == 0)
                                {
                                    Console.WriteLine("\nNo folders here\n");
                                    break;
                                }
                                else
                                {
                                    dirs.Sort();

                                    Console.WriteLine("\n0 \t Up");
                                    for (int i = 0; i < dirs.Count; i++)
                                    {
                                        Console.WriteLine(i + 1 + "\t" + dirs[i]);
                                    }

                                    Console.Write("\nChoose folder to see files/directories in it (e.g. 1f - for files, 1d - for directories, 0 - up): ");

                                    string browseMode = Console.ReadLine();
                                    int id;

                                    if (browseMode == "0")
                                    {
                                        id = 0;
                                        string currDir = dirs[0].Substring(0, dirs[0].LastIndexOf('\\'));
                                        DirectoryInfo parentDir = Directory.GetParent(currDir);

                                        if (parentDir == null)
                                            Console.WriteLine("\nNo parent folder");
                                        else
                                            dirs = Directory.GetDirectories(parentDir.FullName).ToList();
                                    }
                                    else if (browseMode.EndsWith("d") || browseMode.EndsWith("f"))
                                    {
                                        id = Convert.ToInt32(browseMode.Remove(browseMode.Length - 1));

                                        if (browseMode.EndsWith("d"))
                                            dirs = Directory.GetDirectories(dirs[id - 1]).ToList();
                                        else
                                        {
                                            List<string> files = Directory.GetFiles(dirs[id - 1]).ToList();

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
                                    else
                                    {
                                        Console.WriteLine("Ivalid input\n");
                                        break;
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
                        Console.WriteLine("\nInvalid input\n");
                        break;
                }
            }
        }
    }
}
