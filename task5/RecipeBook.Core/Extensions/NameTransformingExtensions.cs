using System.Globalization;
using System.Linq;

namespace RecipeBook.Core.Extensions
{
    public static class NameTransformingExtensions
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
    }
}
