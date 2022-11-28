using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Converters
{
    public class DueDateToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var todoItem = (TodoItem)value;

            if (todoItem.DueDate.HasValue)
            {
                var timestamp = todoItem.DueDate.Value;

                Console.WriteLine("due date convert");

                if (DateTime.Now > timestamp.ToDateTime().ToLocalTime())
                    return Color.Red;



            }
            return Color.Black;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
