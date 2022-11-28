using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinTodoApp.CustomControls;
using XamarinTodoApp.Models;
using XamarinTodoApp.ViewModels.Attribute;

namespace XamarinTodoApp.ViewModels.Todo
{
    public class NewTodoBaseViewModel : ViewModelBase
    {
        protected string _stringFormat;
        protected string selectedRepeatFrequency;
        private string dateEntryText;
        private string description;
        private bool difficultyEasy;
        private bool difficultyHard;
        private bool difficultyMedium;
        private bool difficultyTrivial;
        private DateTime? dueDate;
        private string experienceReward;
        private bool hasNameError;
        private bool isCoinAuto;
        private bool isCoinEntryEnabled;
        private bool isExpAuto;
        private bool isExpEntryEnabled;
        private int maxRewardLength = 6;
        private DateTime? reminderDate;
        private string reminderEntryText;
        private string reward;
        private int selectedDifficulty;
        private string text;
        public NewTodoBaseViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            TestCommand = new Command(DebugCommand);
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            ChangeDifficultyCommand = new Command<int>(OnChangeDifficulty);

            AutoCoinRewardCommand = new Command(GetAutoCoinReward);
            AutoExpRewardCommand = new Command(GetAutoExpReward);

            CustomCoinRewardCommand = new Command(GetCustomCoinReward);
            CustomExpRewardCommand = new Command(GetCustomExpReward);
            ClearDateCommand = new Command(ClearDate);
            ClearReminderCommand = new Command(ClearReminder);
            RepeatCompletedCommand = new Command(OnRepeatCompleted);
            StatSelectedCommand = new Command<AttributeItemViewModel>(OnStatSelected);
            EntryFocusCommand = new Command<MyEntry>(OnEntryFocus);
            ReminderEntryFocusCommand = new Command<MyEntry>(OnReminderEntryFocus);

            IsBusy = true;
        }

        public Command AutoCoinRewardCommand { get; }
        public Command AutoExpRewardCommand { get; }
        public Command CancelCommand { get; }
        public Command<int> ChangeDifficultyCommand { get; }
        public Command ClearDateCommand { get; }
        public Command ClearReminderCommand { get; }

        public Command CustomCoinRewardCommand { get; }
        public Command CustomExpRewardCommand { get; }

        public string DateEntryText
        {
            get => dateEntryText;
            set => SetProperty(ref dateEntryText, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public bool DifficultyEasy
        {
            get => difficultyEasy;
            set => SetProperty(ref difficultyEasy, value);
        }

        public bool DifficultyHard
        {
            get => difficultyHard;
            set => SetProperty(ref difficultyHard, value);
        }

        public bool DifficultyMedium
        {
            get => difficultyMedium;
            set => SetProperty(ref difficultyMedium, value);
        }

        public bool DifficultyTrivial
        {
            get => difficultyTrivial;
            set => SetProperty(ref difficultyTrivial, value);
        }

        public DateTime? DueDate
        {
            get => dueDate;
            set => SetProperty(ref dueDate, value);
        }

        public Command<MyEntry> EntryFocusCommand { get; set; }

        public string ExpReward
        {
            get => experienceReward;
            set
            {
                string cleanString = Helpers.Helpers.RemoveInvalidNumberInput(value);
                experienceReward = experienceReward + "x";
                SetProperty(ref experienceReward, cleanString);
            }
        }

        public bool HasNameError
        {
            get => hasNameError;
            set => SetProperty(ref hasNameError, value);
        }

        public bool IsCoinAuto
        {
            get => isCoinAuto;
            set => SetProperty(ref isCoinAuto, value);
        }

        public bool IsCoinEntryEnabled
        {
            get => isCoinEntryEnabled;
            set => SetProperty(ref isCoinEntryEnabled, value);
        }

        public bool IsExpAuto
        {
            get => isExpAuto;
            set => SetProperty(ref isExpAuto, value);
        }

        public bool IsExpEntryEnabled
        {
            get => isExpEntryEnabled;
            set => SetProperty(ref isExpEntryEnabled, value);
        }

        public AsyncCommand PageAppearingCommand { get; }

        public DateTime? ReminderDate
        {
            get => reminderDate;
            set => SetProperty(ref reminderDate, value);
        }

        public Command<MyEntry> ReminderEntryFocusCommand { get; set; }

        public string ReminderEntryText
        {
            get => reminderEntryText;
            set => SetProperty(ref reminderEntryText, value);
        }

        public Command RepeatCompletedCommand { get; }

        public string Reward
        {
            get => reward;

            set
            {
                string cleanString = Helpers.Helpers.RemoveInvalidNumberInput(value);
                reward = reward + "x";
                SetProperty(ref reward, cleanString);
            }
        }

        public Command SaveCommand { get; }

        public int SelectedDifficulty
        {
            get => selectedDifficulty;
            set => SetProperty(ref selectedDifficulty, value);
        }

        public string SelectedRepeatFrequency
        {
            get => selectedRepeatFrequency;
            set
            {
                
                var isZero = value.All(c => c == '0');

                if (!DueDate.HasValue && !isZero)
                {
                    var defaultDate = DateTime.Now.Date;
                    var defaultTime = new TimeSpan(23, 59, 0);
                    var defaultDateTime = defaultDate.Add(defaultTime);
                    DueDate = defaultDateTime;
                    DateEntryText = defaultDateTime.ToString(StringFormat);
                }


                string cleanString = Helpers.Helpers.RemoveInvalidNumberInput(value);
                selectedRepeatFrequency = selectedRepeatFrequency + "x";
                SetProperty(ref selectedRepeatFrequency, cleanString);
            }
        }

        public Command<AttributeItemViewModel> StatSelectedCommand { get; }

        public string StringFormat
        { get { return _stringFormat ?? "dd/MM/yyyy HH:mm"; } set { _stringFormat = value; } }

        public Command TestCommand { get; }

        public string Text
        {
            get => text;
            set
            {
                SetProperty(ref text, value);
                if (string.IsNullOrWhiteSpace(text))
                {
                    HasNameError = true;
                }
                else
                    HasNameError = false;
            }
        }

        public List<TodoAttribute> TodoAttributes { get; set; }

        public virtual async Task OnAppearing()
        {
            //DueDate = DateTime.Now.Date;
            /*SelectedDifficulty = 1;
            SelectedRepeatFrequency = "";
            DifficultyTrivial = true;
            TodoAttributes.Clear();

            await LoadAttributes();
            GetAutoCoinReward();
            GetAutoExpReward();

            DueDate = null;

            IsBusy = false;*/
        }

        public virtual async void OnSave()
        {
        }

        protected int CalcCoinReward()
        {
            //if (TodoAttributes.Count == 0) return 0;
            return SelectedDifficulty * 15;
        }

        protected int CalcExpReward()
        {
            if (TodoAttributes.Count == 0) return 0;
            return SelectedDifficulty * 10;
        }

        protected void ClearDate()
        {
            DueDate = null;
            DateEntryText = "";
            SelectedRepeatFrequency = "";
        }

        protected void ClearReminder()
        {
            ReminderDate = null;
            ReminderEntryText = "";
        }

        protected void DebugCommand()
        {
            //SelectFilter(FilterType.NoFilter);
        }

        protected void GetAutoCoinReward()
        {
            IsCoinEntryEnabled = false;
            Reward = CalcCoinReward().ToString();
        }

        protected void GetAutoExpReward()
        {
            IsExpEntryEnabled = false;
            ExpReward = CalcExpReward().ToString();
        }

        protected void GetCustomCoinReward()
        {
            IsCoinEntryEnabled = true;
            Reward = "";
        }

        protected void GetCustomExpReward()
        {
            IsExpEntryEnabled = true;
            ExpReward = "";
        }

        protected void OnChangeDifficulty(int difficulty)
        {
            ResetDifficultyButtons();
            SelectedDifficulty = difficulty;

            if (difficulty == 1)
            {
                DifficultyTrivial = true;
            }
            else if (difficulty == 2)
            {
                DifficultyEasy = true;
            }
            else if (difficulty == 3)
            {
                DifficultyMedium = true;
            }
            else if (difficulty == 4)
            {
                DifficultyHard = true;
            }

            if (!IsCoinEntryEnabled)
            {
                Reward = CalcCoinReward().ToString();
            }

            if (!IsExpEntryEnabled)
            {
                ExpReward = CalcExpReward().ToString();
            }
        }

        private void BlockInvalidChar(string value)
        {
            if (value != null && value != "")
            {
                if (!char.IsDigit(value.Last()))
                {
                    SetProperty(ref reward, value.Remove(value.Length - 1, 1));
                }
            }
        }
        /*protected async Task LoadAttributes()
        {
            //SelectFilter(FilterType.BlackAndWhite);
            var userData = App.Current.Resources["userDataVM"] as UserDataViewModel;//?
            foreach (var item in userData.Attributes)
            {
                item.IsSelected = false;
                //SelectFilter(FilterType.BlackAndWhite);
            }
        }*/

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private void OnEntryEvent(object param)
        {
            var focus = param as FocusEventArgs;
            var entry = focus.VisualElement as MyEntry;
            entry.Unfocus();
        }

        private void OnEntryFocus(MyEntry param)
        {
            var entry = param;
            var datePicker = entry.MyDatePicker;
            var timePicker = entry.MyTimePicker;

            datePicker.Unfocused += DateUnfocusHandler;
            timePicker.Unfocused += TimeUnfocusHandler;

            Device.BeginInvokeOnMainThread(() => datePicker.Focus());
            Device.BeginInvokeOnMainThread(() => entry.Unfocus());
            entry.Unfocus();

            void DateUnfocusHandler(object sender, FocusEventArgs args)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    timePicker.Focus();
                    DueDate = datePicker.Date.AddTicks(timePicker.Time.Ticks);
                    DateEntryText = DueDate.Value.ToString(StringFormat);
                });
                datePicker.Unfocused -= DateUnfocusHandler;
            }

            void TimeUnfocusHandler(object sender, FocusEventArgs args)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DueDate = new DateTime(DueDate.Value.Date.Ticks).AddTicks(timePicker.Time.Ticks);
                    DateEntryText = DueDate.Value.ToString(StringFormat);
                });
                timePicker.Unfocused -= TimeUnfocusHandler;
            }
        }

        private void OnReminderEntryFocus(MyEntry param)
        {
            var entry = param;
            var datePicker = entry.MyDatePicker;
            var timePicker = entry.MyTimePicker;

            datePicker.Unfocused += DateUnfocusHandler;
            timePicker.Unfocused += TimeUnfocusHandler;

            Device.BeginInvokeOnMainThread(() => datePicker.Focus());
            Device.BeginInvokeOnMainThread(() => entry.Unfocus());
            entry.Unfocus();

            void DateUnfocusHandler(object sender, FocusEventArgs args)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    timePicker.Focus();
                    ReminderDate = datePicker.Date.AddTicks(timePicker.Time.Ticks);
                    ReminderEntryText = ReminderDate.Value.ToString(StringFormat);
                });
                datePicker.Unfocused -= DateUnfocusHandler;
            }

            void TimeUnfocusHandler(object sender, FocusEventArgs args)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ReminderDate = new DateTime(ReminderDate.Value.Date.Ticks).AddTicks(timePicker.Time.Ticks);
                    ReminderEntryText = ReminderDate.Value.ToString(StringFormat);
                });
                timePicker.Unfocused -= TimeUnfocusHandler;
            }
        }

        private void OnRepeatCompleted(object param)
        {
            var isZero = SelectedRepeatFrequency.All(c => c == '0');
            if (isZero)
                SelectedRepeatFrequency = "";
        }
        private void OnStatSelected(AttributeItemViewModel statAttribute)
        {
            if (!statAttribute.IsSelected && TodoAttributes.Count >= 3) return;
            statAttribute.IsSelected = !statAttribute.IsSelected;
            if (statAttribute.IsSelected)
            {
                TodoAttributes.Add(new TodoAttribute()
                {
                    Id = statAttribute.Item.Id,
                    Name = statAttribute.Item.Name,
                    ImagePath = statAttribute.Item.ImagePath
                });

                //SelectFilter(FilterType.NoFilter);
            }
            else
            {
                TodoAttributes.RemoveAll(x => x.Id == statAttribute.Item.Id);
                //SelectFilter(FilterType.BlackAndWhite);
            }
            CalcCoinReward();
            CalcExpReward();
        }

        private void ResetDifficultyButtons()
        {
            DifficultyTrivial = false;
            DifficultyEasy = false;
            DifficultyMedium = false;
            DifficultyHard = false;
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(reward);
        }
    }
}