using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WPFInvoiceSystem.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolean = (bool)value;

            if (boolean == false)
            {
                if ((parameter as string) == NoVisibilityModes.Hidden) return Visibility.Hidden;
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility)value;

            if (visibility == Visibility.Visible) return true;
            return false;
        }
    }
}
