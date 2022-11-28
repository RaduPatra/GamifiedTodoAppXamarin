using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinTodoApp.Converters
{
    public class BoolToColorConverter : BindableObject , IValueConverter
    {

        public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(BoolToColorConverter));
        public static readonly BindableProperty DefaultColorProperty = BindableProperty.Create(nameof(DefaultColor), typeof(Color), typeof(BoolToColorConverter));

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public Color DefaultColor
        {
            get { return (Color)GetValue(DefaultColorProperty); }
            set { SetValue(DefaultColorProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = (bool)value;
            if (isSelected)
            {
                //return Color.LightGray;
                return SelectedColor;
            }
            //return Color.White;
            return DefaultColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
