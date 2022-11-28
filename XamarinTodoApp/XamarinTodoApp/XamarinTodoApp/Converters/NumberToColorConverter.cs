using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Converters
{
    public class NumberToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var diff = (int)value;

            if (diff == 1)
                return Color.FromHex("#2DDC75");
            else if (diff == 2)
                return Color.FromHex("#4DAB23");
            else if (diff == 3)
                return Color.FromHex("#DE8F19");
            else if (diff == 4)
                return Color.FromHex("#D32F2F");



            return Color.White;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
