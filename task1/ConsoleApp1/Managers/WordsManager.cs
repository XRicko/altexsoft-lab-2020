using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class WordsManager : IShow
    {
        public List<string> Words { get; } = new List<string>();
        public List<string> nthWords { get; } = new List<string>();

        public void GetWords(string text)
        {
            string pattern = @"(\w+)(['`]\w+)?";

            foreach (Match m in Regex.Matches(text, pattern))
            {
                Words.Add(m.Value);
            }
        }
        public void GetEveryNthWord(int nthWord)
        {
            for (int i = 0; i < Words.Count; i++)
            {
                if ((i + 1) % nthWord == 0)
                    nthWords.Add(Words[i]);
            }
        }
        public List<string> ReverseWords(string str)
        {
            string[] sentence = str.Split();
            List<string> output = new List<string>();

            foreach (var item in sentence)
            {
                char[] word = item.ToCharArray();
                Array.Reverse(word);

                string s = new string(word);
                output.Add(s);
            }

            return output;
        }
        public void Show()
        {
            for (int i = 0; i < nthWords.Count; i++)
            {
                if (i == nthWords.Count - 1)
                    Console.WriteLine(nthWords[i] + "\n");
                else
                    Console.Write(nthWords[i] + ", ");
            }
        }
    }
}
