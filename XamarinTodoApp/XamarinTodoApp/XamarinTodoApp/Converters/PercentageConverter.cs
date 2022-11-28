using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;
namespace XamarinTodoApp.Converters
{
    public class PercentageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is string)
            {
                var num = float.Parse((string)value);
                var perc1 = num * 100;
                return ((int)perc1).ToString();
            }


            var number = (float)value;
            var perc = number * 100;
            return ((int)perc).ToString();



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
