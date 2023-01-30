using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Utility
{
    public static class Extensions
    {
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }

        public static bool IsInvalidCharactor(char ch)
        {
            return !(char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch));
        }
    }
}
