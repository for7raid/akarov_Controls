using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace akarov.Controls.MVVM.Converters
{
    public class InvertorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val;
            if (bool.TryParse(value.ToString(), out val))
            {
                return !val;
            }
            else
                throw new FormatException(string.Format("Значение {0} не может быть преобразовано к типу bool",value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
