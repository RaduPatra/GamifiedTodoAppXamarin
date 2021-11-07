using Firebase.Database;
using Google.Cloud.Firestore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.Views;

namespace XamarinTodoApp.ViewModels.Todo
{
    public class NewTodoViewModel : ViewModelBase
    {
        private string text;
        private string reward;
        private string experienceReward;

        public NewTodoViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(reward);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Reward
        {
            get => reward;
            set => SetProperty(ref reward, value);
        }

        public string ExpReward
        {
            get => experienceReward;
            set => SetProperty(ref experienceReward, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var fbkey = FirebaseKeyGenerator.Next();
            DateTime date = DateTime.Now.ToUniversalTime();
            TodoItem newItem = new TodoItem()
            {
                Id = "",
                Text = Text,
                IsDone = false,
                Reward = int.Parse(Reward),
                ExpReward = int.Parse(ExpReward),
                CreationDate = Timestamp.FromDateTime(date)
            };

            await TodoStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
