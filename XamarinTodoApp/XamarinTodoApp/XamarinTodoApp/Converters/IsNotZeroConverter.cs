using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
namespace XamarinTodoApp.Converters
{
    public class IsNotZeroConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var number = (int)value;

            if (number == 0)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
