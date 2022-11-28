using Google.Cloud.Firestore;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinTodoApp.CustomControls;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.ViewModels.Attribute;
using XamarinTodoApp.ViewModels.General;

namespace XamarinTodoApp.ViewModels.Todo
{
    public class NewTodoViewModel : NewTodoBaseViewModel
    {


        public NewTodoViewModel()
        {
            TodoAttributes = new List<TodoAttribute>();
        }


        //public List<TodoAttribute> TodoAttributes { get; set; }

        public override async Task OnAppearing()
        {
            //DueDate = DateTime.Now.Date;
            await base.OnAppearing();
            SelectedDifficulty = 1;
            SelectedDifficulty = 1;
            SelectedRepeatFrequency = "";
            DifficultyTrivial = true;
            TodoAttributes.Clear();

            await LoadAttributes();

            GetAutoCoinReward();
            GetAutoExpReward();

            DueDate = null;

            IsBusy = false;
        }


        private async Task LoadAttributes()
        {
            //SelectFilter(FilterType.BlackAndWhite);
            var userData = App.Current.Resources["userDataVM"] as UserDataViewModel;//?
            foreach (var item in userData.Attributes)
            {
                item.IsSelected = false;
                //SelectFilter(FilterType.BlackAndWhite);
            }
        }
        public override async void OnSave()
        {
            DateTime creationDate = DateTime.Now.ToUniversalTime();

            Timestamp? dueDateTimestamp = null;
            Timestamp? reminderDateTimestamp = null;

            if (DueDate.HasValue)
            {
                DateTime? dueDate = DueDate.Value.ToUniversalTime();
                dueDateTimestamp = Timestamp.FromDateTime(dueDate.Value);
            }

            if (ReminderDate.HasValue)
            {
                DateTime? reminderDate = ReminderDate.Value.ToUniversalTime();
                reminderDateTimestamp = Timestamp.FromDateTime(reminderDate.Value);
            }

            var testDueTimestamp = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime().AddSeconds(10));

            int repFreq;
            NotificationRepeat repeatType;
            if (SelectedRepeatFrequency == "")
            {
                repFreq = 0;
                repeatType = NotificationRepeat.No;
            }
            else
            {
                repFreq = int.Parse(SelectedRepeatFrequency);
                repeatType = NotificationRepeat.TimeInterval;
            }

            var notificationId = (int)(DateTime.Now.Ticks >> 10);
            TodoItem newItem = new TodoItem()
            {
                Id = "",
                Text = Text,
                Description = Description,
                IsDone = false,
                Reward = int.Parse(Reward),
                ExpReward = int.Parse(ExpReward),
                CreationDate = Timestamp.FromDateTime(creationDate),
                Difficulty = SelectedDifficulty,
                StatAttributes = TodoAttributes,
                DueDate = dueDateTimestamp,
                ReminderDate = reminderDateTimestamp,
                RepeatFrequency = repFreq,
                HasCustomCoinReward = IsCoinEntryEnabled,
                HasCustomExpReward = IsExpEntryEnabled,
                NotificationId = notificationId
            };

            //NotificationCenter.Current.CancelAll();
            //var repeatType = NotificationRepeat.No;

            if (ReminderDate.HasValue)
            {
                var notification = new NotificationRequest
                {
                    BadgeNumber = 1,
                    Description = Text,
                    Title = "Reminder",
                    //ReturningData = "Dummy Data",
                    NotificationId = notificationId,

                    Schedule =
                    {
                        NotifyTime = ReminderDate,
                        RepeatType = repeatType,
                        NotifyRepeatInterval = new TimeSpan(24 * repFreq, 0,0)
                    }
                };
                await NotificationCenter.Current.Show(notification);
            }

            // await TodoStore.AddItemAsync(newItem);
            await TodoStore.AddTodoAsync(newItem, AuthService.UserInfo.LocalId);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Text)
                && !String.IsNullOrWhiteSpace(Reward);
        }
    }
}