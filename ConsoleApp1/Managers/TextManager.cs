using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class TextManager
    {
        public string Text { get; set; }

        public string RemoveSymbols(string symbols)
        {
            string output = Regex.Replace(Text, symbols, "", RegexOptions.IgnoreCase);

            if (String.Compare(Text, output) != 0)
                return output;
            else
                return null;
        }
    }
}
