using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RecipeBook.SharedKernel.Extensions
{
    public static class StringTransformingExtensions
    {
        public static string StandardizeName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()).Trim();
        }

        public static IList<string> GetWords(this string text)
        {
            var words = new List<string>();

            string pattern = @"([\w]+(['`]\w+)?([ \w]+)?)";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                words.Add(m.Value);
            }

            return words;
        }
    }
}
