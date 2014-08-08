using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace akarov.Controls.Extensions
{
    public static partial class Extension
    {
        public static string f(this string input,params object[] args)
        {
            return String.Format(input, args);
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }
    }
}
