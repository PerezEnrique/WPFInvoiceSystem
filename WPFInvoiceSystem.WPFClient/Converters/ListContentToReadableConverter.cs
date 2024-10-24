using System;
using System.Globalization;
using System.Windows.Data;
using WPFInvoiceSystem.WPFClient.Utils.Enums;

namespace WPFInvoiceSystem.WPFClient.Converters
{
    public class ListContentToReadableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) return string.Empty;

            ListContents listContent = (ListContents)value;

            switch (listContent)
            {
                case ListContents.All:
                    return "all items";
                case ListContents.LastTenItems:
                    return "last ten items";
                case ListContents.SearchResults:
                    return "filtered results";
                default:
                    throw new ArgumentException($"Unexpected list content value: {listContent}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
