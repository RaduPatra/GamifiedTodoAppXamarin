using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace XamarinTodoApp.CustomControls
{
    public class NullableDatePickerTest : DatePicker
    {
        public static readonly BindableProperty NullableDateProperty
            = BindableProperty.Create(
                nameof(NullableDate),
                typeof(DateTime?),
                typeof(NullableDatePicker),
                null);

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); UpdateDate(); }
        }
        private string _format = null;

        private void UpdateDate()
        {
            if (NullableDate.HasValue)
            {
                if (NullableDate == DateTime.Today)
                {
                    _format = Format;
                    Format = "Pick a date...";
                }
                else
                {
                    if (null != _format)
                        Format = _format;
                    Date = NullableDate.Value;
                }
            }
            else { _format = Format; Format = "Pick a date..."; }
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateDate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Date") NullableDate = Date;
        }
    }
}
