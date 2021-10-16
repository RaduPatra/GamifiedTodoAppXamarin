using Firebase.Database;
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
            TodoItem newItem = new TodoItem()
            {
                Id = fbkey,
                Text = Text,
                IsDone = false,
                Reward = int.Parse(Reward)
            };

            await TodoStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
