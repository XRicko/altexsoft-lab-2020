using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Managers
{
    class ProgramManager
    {
        ConsoleInputReader inputData = new ConsoleInputReader();

        FoldersManager foldersManager = new FoldersManager();
        FilesManager filesManager = new FilesManager();
        TextManager textManager = new TextManager();
        SentencesManager sentencesManager = new SentencesManager();
        WordsManager wordsManager = new WordsManager();

        private void ReplaceSymbols()
        {
            string originalFileName = Path.GetFileName(filesManager.file.Path);

            if (!File.Exists(originalFileName))
                File.Copy(filesManager.file.Path, originalFileName);

            string symbols = inputData.CaptureSymbols();
            string replaced = textManager.RemoveSymbols(symbols);

            if (replaced != null)
            {
                filesManager.WriteTextToFile(replaced);
                Console.WriteLine("\nDone");
            }
            else
                ConsoleMessages.Failure("such symbol(s) in the text");
        }
        private void NumberOfWords()
        {
            wordsManager.GetWords(textManager.Text);
            Console.WriteLine("\nWords in text: " + wordsManager.Words.Count + "\n");
        }
        private void EveryNthWord()
        {
            int nthWord = 10;
            wordsManager.GetEveryNthWord(nthWord);

            Console.Write($"Every {nthWord} word: ");
            wordsManager.Show();
        }
        private void SentenceBackwards()
        {
            sentencesManager.GetSentences(textManager.Text);
            int nthSentence = 3;
            sentencesManager.sentence.Text = wordsManager.ReverseWords(sentencesManager.Sentences[nthSentence - 1]);

            Console.Write($"\n{nthSentence} sentence backwards: ");
            sentencesManager.Show();
            Console.WriteLine();
        }
        private void Browse()
        {
            foldersManager.Show();

            string browseMode = inputData.CaptureBrowseMode();

            if (browseMode == "0")
                BrowseUp();
            else if (browseMode.EndsWith("d") || browseMode.EndsWith("f"))
            {
                int id = Convert.ToInt32(browseMode.Remove(browseMode.Length - 1));

                if (browseMode.EndsWith("d"))
                    foldersManager.GoNext(id);
                else
                    BrowseFiles(id);
            }
            else
            {
                ConsoleMessages.ValidationError("input");
                return;
            }
        }
        private void BrowseUp()
        {
            foldersManager.GetParentDir();

            if (foldersManager.folder.ParentDir == null)
                ConsoleMessages.Failure("parent folder");
            else
                foldersManager.GoBack();
        }
        private void BrowseFiles(int id)
        {
            filesManager.GetFiles(foldersManager.Dirs[id - 1]);

            if (filesManager.Files.Count == 0)
                ConsoleMessages.Failure("files");
            else
                filesManager.Show();
        }
        private void Mode1Proccess()
        {
            filesManager.file.Path = inputData.CapturePath("file");
            textManager.Text = filesManager.GetTextFromFile();

            Console.Clear();

            string option = inputData.CaptureOption();

            if (option == "1")
                ReplaceSymbols();
            else if (option == "2")
            {
                NumberOfWords();
                EveryNthWord();
            }
            else if (option == "3")
                SentenceBackwards();
            else
                ConsoleMessages.ValidationError("input");
        }
        private void Mode2Proccess()
        {
            foldersManager.folder.Path = inputData.CapturePath("folder");
            foldersManager.Dirs = Directory.GetDirectories(foldersManager.folder.Path).ToList();

            while (true)
            {
                if (foldersManager.Dirs.Count == 0)
                {
                    ConsoleMessages.Failure("folders");
                    break;
                }
                else
                    Browse();
            }
        }
        public void Proccess()
        {
            while (true)
            {
                string mode = inputData.CaptureMode();

                switch (mode)
                {
                    case "1":
                        Console.Clear();

                        try
                        {
                            Mode1Proccess();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "\n");
                        }

                        break;
                    case "2":
                        Console.Clear();

                        try
                        {
                            Mode2Proccess();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "\n");
                        }

                        break;
                    case "3":
                        return;
                    default:
                        ConsoleMessages.ValidationError("input");
                        break;
                }
            }
        }
    }
}
