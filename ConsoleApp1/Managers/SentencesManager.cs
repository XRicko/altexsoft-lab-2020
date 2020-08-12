using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class SentencesManager : IShow
    {
        public SentenceModel sentence = new SentenceModel();
        public string[] Sentences { get; set; }

        public void GetSentences(string text)
        {
            Sentences = Regex.Split(text, @"[.?!]");
        }
        public void Show()
        {
            foreach (var item in sentence.Text)
            {
                Console.Write(item + " ");
            }
        }
    }
}
