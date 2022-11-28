using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using System.ComponentModel;
using XamarinTodoApp.CustomControls;
using XamarinTodoApp.Droid.Renderers;

[assembly: ExportRenderer(typeof(NullableDatePicker), typeof(NullableDatePickerRenderer))]
namespace XamarinTodoApp.Droid.Renderers
{
    
    public class NullableDatePickerRenderer : ViewRenderer<NullableDatePicker, EditText>
    {
        DatePickerDialog _dialog;


        public NullableDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NullableDatePicker> e)
        {
            base.OnElementChanged(e);
            this.SetNativeControl(new Android.Widget.EditText(Context));
            this.Element.MinimumDate = new DateTime(2021, 10, 10);
            if (Control == null || e.NewElement == null)
                return;
            var entry = (NullableDatePicker)this.Element;

            this.Control.Click += OnPickerClick;
            this.Control.Text = !entry.NullableDate.HasValue ? entry.PlaceHolder : Element.Date.ToString(Element.Format);
            this.Control.KeyListener = null;
            this.Control.FocusChange += OnPickerFocusChange;
            this.Control.Enabled = Element.IsEnabled;

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                var entry = (NullableDatePicker)this.Element;

                if (this.Element.Format == entry.PlaceHolder)
                {
                    this.Control.Text = entry.PlaceHolder;
                    return;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        void OnPickerFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
                //OnFocused(sender, e);

            }
            else
            {
                //OnUnfocused(sender, e);
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;
                this.Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();  
        }

        void SetDate(DateTime date)
        {
            this.Control.Text = date.ToString(Element.Format);

            Element.Date = date;

        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(this.Element.Date.Year, this.Element.Date.Month - 1, this.Element.Date.Day);
            _dialog.Show();

        }

        void CreateDatePickerDialog(int year, int month, int day)
        {
            NullableDatePicker view = Element;
            _dialog = new DatePickerDialog(Context, (o, e) =>
            {
                view.Date = e.Date;
                //((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();
                
                _dialog = null;
            }, year, month, day);

            _dialog.SetButton("Done", (sender, e) =>
            {
                this.Element.Format = this.Element._originalFormat;
                SetDate(_dialog.DatePicker.DateTime);
                this.Element.AssignValue();
                ((IElementController)view).SetValueFromRenderer(Entry.IsFocusedPropertyKey, false);
                
            });
            _dialog.SetButton2("Clear", (sender, e) =>
            {
                this.Element.CleanDate();
                Control.Text = this.Element.Format;
            });
            ((IElementController)view).SetValueFromRenderer(Entry.IsFocusedPropertyKey, true);
            _dialog.SetOnCancelListener(new MyCancelListener(view, Control));

            
            
        }
        private IElementController ElementController => Element as IElementController;
        private void OnFocused()
        {
            ElementController.SetValueFromRenderer(Entry.IsFocusedPropertyKey, true);
        }

        private void OnUnfocused()
        {
            ElementController.SetValueFromRenderer(Entry.IsFocusedPropertyKey, false);
        }

    }

    public class MyCancelListener
    : Java.Lang.Object, IDialogInterfaceOnCancelListener
    {

        NullableDatePicker picker;
        EditText control;
        public MyCancelListener(/* you could pass stuff here */ NullableDatePicker picker, EditText control)
        {
            this.picker = picker;
            this.control = control;
        }

        public void OnCancel(IDialogInterface dialog)
        {
            //Do stuff when cancelled
            //Maybe with stuff from ctor
            // OnUnfocused(sender, e);
            //picker.SetValueFromRenderer(Entry.IsFocusedPropertyKey, false);

            this.picker.CleanDate();
            control.Text = this.picker.Format;
            ((IElementController)picker).SetValueFromRenderer(Entry.IsFocusedPropertyKey, false);


        }
    }
}