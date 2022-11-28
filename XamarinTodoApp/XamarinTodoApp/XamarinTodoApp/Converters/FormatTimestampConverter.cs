using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinTodoApp.Converters
{
    public class FormatTimestampConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return "";
            var timestamp = (Timestamp)value;
            var date = timestamp.ToDateTime().ToLocalTime();
            return date.ToString("dd/MM/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
