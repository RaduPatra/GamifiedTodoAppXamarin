using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTodoApp.CustomControls;

namespace XamarinTodoApp.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            //cDatePicker.IsVisible = false;
            //cDateEntry.Placeholder = "test";
            //cClearDateButton.Text = ClearDateText;
        }


        //    #region PROPERTIES
        //    public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(NullableDatePickerTest2), null, BindingMode.TwoWay);
        //    public static readonly BindableProperty NoDatePlaceholderProperty = BindableProperty.Create(nameof(NoDatePlaceholder), typeof(string), typeof(NullableDatePickerTest2), "No Date Selected");
        //    public static readonly BindableProperty DateFormatProperty = BindableProperty.Create(nameof(DateFormat), typeof(string), typeof(NullableDatePickerTest2), "MM/dd/yyyy");
        //    public static readonly BindableProperty ClearDateTextProperty = BindableProperty.Create(nameof(ClearDateText), typeof(string), typeof(NullableDatePickerTest2), "Clear");
        //    public static readonly BindableProperty IsDateNullableProperty = BindableProperty.Create(nameof(IsDateNullable), typeof(bool), typeof(NullableDatePickerTest2), true);

        //    public DateTime? SelectedDate
        //    {
        //        get { return (DateTime?)GetValue(SelectedDateProperty); }
        //        set { SetValue(SelectedDateProperty, value); }
        //    }

        //    public string NoDatePlaceholder
        //    {
        //        get { return (string)GetValue(NoDatePlaceholderProperty); }
        //        set { SetValue(NoDatePlaceholderProperty, value); }
        //    }

        //    public string DateFormat
        //    {
        //        get { return (string)GetValue(DateFormatProperty); }
        //        set { SetValue(DateFormatProperty, value); }
        //    }

        //    public string ClearDateText
        //    {
        //        get { return (string)GetValue(ClearDateTextProperty); }
        //    }

        //    public bool IsDateNullable
        //    {
        //        get { return (bool)GetValue(IsDateNullableProperty); }
        //        set { SetValue(IsDateNullableProperty, value); }
        //    }

        //    protected override void OnPropertyChanged(string propertyName = null)
        //    {
        //        base.OnPropertyChanged(propertyName);

        //        if (propertyName == SelectedDateProperty.PropertyName)
        //        {
        //            if (SelectedDate == null)
        //            {
        //                cDateEntry.Text = null;
        //                cDateEntry.Placeholder = NoDatePlaceholder;
        //            }
        //            else
        //            {
        //                cDatePicker.Date = SelectedDate.GetValueOrDefault();
        //                cDateEntry.Text = SelectedDate.GetValueOrDefault().ToString(DateFormat);
        //            }
        //        }
        //        else if (propertyName == NoDatePlaceholderProperty.PropertyName)
        //        {
        //            cDateEntry.Placeholder = NoDatePlaceholder;
        //        }
        //        else if (propertyName == ClearDateTextProperty.PropertyName)
        //        {
        //            cClearDateButton.Text = ClearDateText;
        //        }
        //        else if (propertyName == IsDateNullableProperty.PropertyName)
        //        {
        //            if (!IsDateNullable)
        //            {
        //                cClearDateButton.IsEnabled = false;
        //                cClearDateButton.IsVisible = false;
        //            }
        //        }
        //    }

        //    #endregion PROPERTIES


        //    private void cDateEntry_Focused(object sender, FocusEventArgs e)
        //    {
        //        //cDateEntry.Unfocus();
        //        //cDatePicker.IsVisible = true;
        //        cDatePicker.Focus();
        //    }

        //    private void cDatePicker_Focused(object sender, FocusEventArgs e)
        //    {
        //        if (SelectedDate == null)
        //        {
        //            //SelectedDate = cDatePicker.Date;
        //        }
        //    }

        //    private void cDatePicker_Unfocused(object sender, FocusEventArgs e)
        //    {
        //        cDatePicker.IsVisible = false;
        //    }

        //    private void cDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        //    {
        //        SelectedDate = e.NewDate;
        //    }

        //    private void cClearDateButton_Clicked(object sender, EventArgs e)
        //    {
        //        SelectedDate = null;
        //    }
        //}
    }
}