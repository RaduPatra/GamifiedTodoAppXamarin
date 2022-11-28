using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

using XamarinTodoApp.Helpers;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using System.ComponentModel;
using XamarinTodoApp.CustomControls;

namespace XamarinTodoApp.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AsyncCommand PageAppearingCommand { get; }
        public Command TestCommand { get; }
        public Command TestCommand2 { get; }
        public Command EntryEventToCommand { get; }
        public Command<MyEntry> EntryFocusCommand { get; set; }

        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            //TestLoginInfo();
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            TestCommand = new Command(DebugCommand);
            TestCommand2 = new Command(DebugCommand2);
            EntryEventToCommand = new Command(OnEntryEvent);
            EntryFocusCommand = new Command<MyEntry>(OnEntryFocus);

        }
        public ICommand OpenWebCommand { get; }

        private DateTime? dueDate;
        public DateTime? DueDate
        {
            get => dueDate;
            set => SetProperty(ref dueDate, value);
        }

        private TimeSpan DueTime
        {
            get
            {
                return TimeSpan.FromTicks(DueDate.Value.Ticks);
            }
            set
            {
                DueDate = new DateTime(DueDate.Value.Date.Ticks).AddTicks(value.Ticks);
            }
        }

        string _stringFormat { get; set; }
        public string StringFormat { get { return _stringFormat ?? "dd/MM/yyyy HH:mm"; } set { _stringFormat = value; } }


        public bool IsFocused
        {
            get;
            set;
        }

        void OnEntryEvent(object param)
        {
            
            var focus = param as FocusEventArgs;
            var entry = focus.VisualElement as MyEntry;
            entry.Unfocus();
        }
        void OnEntryFocus(MyEntry param)
        {
            //var focus = param as FocusEventArgs;
            //var focus = param;
            //var entry = focus.VisualElement as MyEntry;
            var entry = param;
            var datePicker = entry.MyDatePicker;
            var timePicker = entry.MyTimePicker;

            
            Device.BeginInvokeOnMainThread(() => datePicker.Focus());
            
            entry.MyDatePicker.Unfocused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    timePicker.Focus();
                    DueDate = datePicker.Date.AddTicks(timePicker.Time.Ticks);
                    entry.Text = DueDate.Value.ToString(StringFormat);
                });
            };


            timePicker.Unfocused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DueTime = timePicker.Time;
                    entry.Text = DueDate.Value.ToString(StringFormat);
                });
;
            };
            Device.BeginInvokeOnMainThread(() => entry.Unfocus());
            entry.Unfocus();

        }

        private void UpdateEntryText()
        {
        }
        void DebugCommand()
        {
            //SelectFilter(FilterType.BlackAndWhite);
        }

        void DebugCommand2()
        {
            SelectFilter(FilterType.NoFilter);
        }
        public Command<FilterType> SelectFilterCommand { get; set; }

        //public ICommand ApplyFilterCommand { get; set; }



        //public event PropertyChangedEventHandler PropertyChanged;

        public void SelectFilter(FilterType filter)
        {
            SelectFilterCommand.Execute(filter);
        }

        public async Task OnAppearing()
        {
           // EntryFocusCommand = new Command(OnEntryFocus);
        }
    }
}