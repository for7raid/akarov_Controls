using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace akarov.Controls.Extensions
{
    public static partial class Extension
    {
        public static string F(this string input,params object[] args)
        {
            return String.Format(input, args);
        }
    }
}
