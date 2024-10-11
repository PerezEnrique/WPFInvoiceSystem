using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFInvoiceSystem.WPFClient.Converters
{
    public class BooleanToYesOrNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean boolean = (bool)value;

            if (boolean == false) return "No";
            return "Yes";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
