using System;

namespace ConsoleApp1
{
    class ConsoleInputReader
    {
        public string CapturePath(string obj)
        {
            ConsoleMessages.Request($"path to the {obj}");
            string output = Console.ReadLine();

            return output;
        }
        public string CaptureMode()
        {
            ConsoleMessages.Request("what you wanna do: \n 1 - work with text file \n 2 - browse through folders \n 3 - exit \n");
            string output = Console.ReadLine();

            return output;
        }
        public string CaptureOption()
        {
            ConsoleMessages.Request("an option: \n 1 - remove symbols in text \n 2 - number of words in text and see every 10th word \n 3 - 3rd sentence backwards \n");
            string output = Console.ReadLine();

            return output;
        }
        public string CaptureSymbols()
        {
            ConsoleMessages.Request("symbol(s) you want to remove");
            string output = Console.ReadLine();

            return output;
        }
        public string CaptureBrowseMode()
        {
            ConsoleMessages.Request("folder to see files/directories in it (e.g. 1f - for files, 1d - for directories, 0 - up)");
            string output = Console.ReadLine();

            return output;
        }
    }
}
