using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinTodoApp.Triggers
{
    public class FocusTriggerAction : TriggerAction<DatePicker>
    {
        public bool Focused { get; set; }
        public TimePicker timePicker { get; set; }
        //public bool Focused { get; set; }

        protected override async void Invoke(DatePicker searchBar)
        {
            //await Task.Delay(1000);

            if (Focused)
            {
                searchBar.Focus();
            }
            else
            {
                searchBar.Unfocus();
            }
        }
    }
}
