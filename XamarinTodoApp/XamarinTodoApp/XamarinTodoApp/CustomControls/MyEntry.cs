using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinTodoApp.CustomControls
{
    public class MyEntry : Entry
    {

        public static readonly BindableProperty TimePickerProperty =
        BindableProperty.Create(nameof(MyTimePicker), typeof(TimePicker), typeof(MyEntry), null, defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty DatePickerProperty =
        BindableProperty.Create(nameof(MyDatePicker), typeof(DatePicker), typeof(MyEntry), null, defaultBindingMode: BindingMode.TwoWay);


        public TimePicker MyTimePicker
        {
            get { return (TimePicker)GetValue(TimePickerProperty); }
            set { SetValue(TimePickerProperty, value); }
        }

        public DatePicker MyDatePicker
        {
            get { return (DatePicker)GetValue(DatePickerProperty); }
            set { SetValue(DatePickerProperty, value); }
        }
    }
}
