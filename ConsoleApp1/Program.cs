using ConsoleApp1.Managers;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramManager programManager = new ProgramManager();
            programManager.Proccess();
        }
    }
}
