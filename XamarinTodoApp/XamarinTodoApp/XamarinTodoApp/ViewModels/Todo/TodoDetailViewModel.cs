using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.Views;
using Google.Cloud.Firestore;

namespace XamarinTodoApp.ViewModels.Todo
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class TodoDetailViewModel : ViewModelBase
    {
        private string itemId;
        private string text;
        private string reward;
        private TodoItem Item { get; set; }
        public string Id { get; set; }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }


        public TodoDetailViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private async void OnSave()
        {
            DateTime date = DateTime.Now.ToUniversalTime();
            TodoItem newItem = new TodoItem()
            {
                Id = ItemId,
                Text = Text,
                IsDone = Item.IsDone,
                Reward = int.Parse(Reward),
                CreationDate = Item.CreationDate
            };

            await TodoStore.UpdateItemAsync(ItemId, newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
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

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await TodoStore.GetItemAsync(itemId);
                Item = item;
                Id = item.Id;
                Text = item.Text;
                Reward = item.Reward.ToString();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}

