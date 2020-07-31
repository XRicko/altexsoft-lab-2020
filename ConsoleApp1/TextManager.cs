using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    static class TextManager
    {
        public static string ReverseString(string str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);

            return new string(array);
        }
        public static string RemoveSymbols(string text, string symbols)
        {
            string newText = Regex.Replace(text, symbols, "", RegexOptions.IgnoreCase);

            if (String.Compare(text, newText) != 0)
                return newText;
            else
                return null;
        }
        public static string DeletePunctuation(string str)
        {
            string noPunctText = Regex.Replace(str, "[-.?!)(,:“”]", "");

            return noPunctText;

        }
        public static List<string> GetEveryNthWord(string[] words, int nthWord)
        {
            List<string> nWords = new List<string>();

            for (int i = 0; i < words.Length; i++)
            {
                if ((i + 1) % nthWord == 0)
                    nWords.Add(words[i]);
            }

            return nWords;
        }
    }
}
