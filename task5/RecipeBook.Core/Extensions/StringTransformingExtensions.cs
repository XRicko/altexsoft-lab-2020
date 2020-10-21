using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecipeBook.Core.Extensions
{
    public static class StringTransformingExtensions
    {
        public static string StandardizeName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()).Trim();
        }

        public static string RemoveDublicates(this string name)
        {
            return string.Join(" ", name.Split(' ').Distinct());
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
