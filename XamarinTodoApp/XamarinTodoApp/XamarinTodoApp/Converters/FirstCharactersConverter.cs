using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinTodoApp.Converters
{
    public class FirstCharactersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;

            if (s.Length >= 3)
                return s.ToUpper().Substring(0, 3);
            else return s.ToUpper();    
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
