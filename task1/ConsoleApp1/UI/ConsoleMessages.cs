using System;

namespace ConsoleApp1
{
    static class ConsoleMessages
    {
        public static void Failure(string obj)
        {
            Console.WriteLine($"\nNo {obj}\n");
        }
        public static void ValidationError(string obj)
        {
            Console.WriteLine($"\nInvalid {obj}\n");
        }
        public static void Request(string text)
        {
            Console.Write($"\nType {text}: ");
        }
    }
}
