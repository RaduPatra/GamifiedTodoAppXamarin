using Google.Cloud.Firestore;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinTodoApp.CustomControls;
using XamarinTodoApp.Models;
using XamarinTodoApp.ViewModels.Attribute;
using XamarinTodoApp.ViewModels.General;

namespace XamarinTodoApp.ViewModels.Todo
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    internal class TodoDetailViewModel : NewTodoBaseViewModel
    {

       
        private string itemId;
        

        public TodoDetailViewModel()
        {

        }




        public string Id { get; set; }

      
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

      
        public new string SelectedRepeatFrequency
        {
            get => selectedRepeatFrequency;
            set
            {
                var isZero = value.All(c => c == '0');

                if (!DueDate.HasValue && !isZero && !IsBusy)
                {
                    var defaultDate = DateTime.Now.Date;
                    var defaultTime = new TimeSpan(23, 59, 0);
                    var defaultDateTime = defaultDate.Add(defaultTime);
                    DueDate = defaultDateTime;
                    DateEntryText = defaultDateTime.ToString(StringFormat);
                }

                //if (isZero)
                //    value = "";
                SetProperty(ref selectedRepeatFrequency, value);
            }
        }


        private void LoadAttributes()
        {
            //SelectFilter(FilterType.BlackAndWhite);	
            var userData = App.Current.Resources["userDataVM"] as UserDataViewModel;//?	
            foreach (var item in userData.Attributes)
            {
                item.IsSelected = false;
            }
            foreach (var item in TodoAttributes)
            {
                var stat = userData.Attributes.ToList().Find(st => st.Item.Id == item.Id);
                stat.IsSelected = true;
                //var stat = userData.Attributes.Find(st => st.Id == attr.Id);	
            }
        }

        private TodoItem Item { get; set; }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await TodoStore.GetItemAsync(itemId);
                Item = item;
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
                Reward = item.Reward.ToString();
                ExpReward = item.ExpReward.ToString();
                SelectedDifficulty = item.Difficulty;
                TodoAttributes = item.StatAttributes;
                IsCoinEntryEnabled = item.HasCustomCoinReward;
                IsExpEntryEnabled = item.HasCustomExpReward;

                LoadAttributes();
                OnChangeDifficulty(SelectedDifficulty);

                if (item.DueDate.HasValue)
                {
                    var date = item.DueDate.Value.ToDateTime().ToLocalTime();
                    DueDate = date;
                    DateEntryText = date.ToString(StringFormat);
                }
                else DueDate = null;

                if (item.ReminderDate.HasValue)
                {
                    var date = item.ReminderDate.Value.ToDateTime().ToLocalTime();
                    ReminderDate = date;
                    ReminderEntryText = date.ToString(StringFormat);
                }
                else ReminderDate = null;

                SelectedRepeatFrequency = item.RepeatFrequency.ToString();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
            IsBusy = false;
        }

        public override async Task OnAppearing()
        {

        }

        public override async void OnSave()
        {
            //DateTime creationDate = DateTime.Now.ToUniversalTime();

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

            TodoItem newItem = Item;
            newItem.Text = Text;
            newItem.Description = Description;
            newItem.Reward = int.Parse(Reward);
            newItem.ExpReward = int.Parse(ExpReward);
            newItem.Difficulty = SelectedDifficulty;
            newItem.StatAttributes = TodoAttributes;
            newItem.DueDate = dueDateTimestamp;
            newItem.ReminderDate = reminderDateTimestamp;
            newItem.RepeatFrequency = repFreq;
            newItem.HasCustomCoinReward = IsCoinEntryEnabled;
            newItem.HasCustomExpReward = IsExpEntryEnabled;

            if (ReminderDate.HasValue)
            {
                var notification = new NotificationRequest
                {
                    BadgeNumber = 1,
                    Description = Text,
                    Title = "Reminder",
                    //ReturningData = "Dummy Data",
                    NotificationId = Item.NotificationId,

                    Schedule =
                    {
                        NotifyTime = ReminderDate,
                        RepeatType = repeatType,
                        NotifyRepeatInterval = new TimeSpan(24 * repFreq, 0,0)
                    }
                };
                await NotificationCenter.Current.Show(notification);
            }
            else
            {
                NotificationCenter.Current.Cancel(Item.NotificationId);
            }

            // await TodoStore.AddItemAsync(newItem);
            await TodoStore.UpdateItemAsync(ItemId, newItem);
            //await TodoStore.AddTodoAsync(newItem, AuthService.UserInfo.LocalId);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

    }
}