using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace akarov.Controls.MVVM.Converters
{
    public static class Converters
    {
        public static IValueConverter BoolenToVisibility { get { return new BoolenToVisibilityConverter(); } }
        public static IValueConverter Invertor { get { return new InvertorConverter(); } }
    }
}
