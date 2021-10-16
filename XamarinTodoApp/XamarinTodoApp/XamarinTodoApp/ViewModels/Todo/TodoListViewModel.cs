
//using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.ViewModels.Todo;
//using XamarinTodoApp.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinTodoApp.Views.Todo;

namespace XamarinTodoApp.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {

        private readonly string tempId = "-Mll4lTYtKLYS4viYp8x";
        private TodoItem _selectedItem;
        private string coins;

        public ObservableCollection<TodoItem> Items { get; }
        public AsyncCommand LoadItemsCommand { get; }
        public Command CheckedCommand { get; }
        public Command AddItemCommand { get; }
        public Command<TodoItem> ItemTapped { get; }

        public Command TestCommand { get; }


        public TodoListViewModel()
        {
            IsBusy = true;
            Console.WriteLine("CONSTR");
            Title = "Todo browse";
            Items = new ObservableCollection<TodoItem>();
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadItemsCommand = new AsyncCommand(ExecuteLoadItemsCommand);

            //ItemTapped = new Command<TodoItem>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            CheckedCommand = new Command<TodoItem>(async (item) => await ExecuteChecked(item));
            //CheckedCommand = new AsyncCommand<TodoItem>(ExecuteChecked);
        }
        void TestCom() { return; }


        private async Task ExecuteChecked(TodoItem item)
        {

            if (IsBusy || item == null) return;
            Console.WriteLine("EXEC CHECKED ");
            await TodoStore.UpdateItemAsync(item.Id, item);

            UserData userData = await GetUserDataAsync();
            if (item.IsDone)
            {
                userData.Coins += item.Reward;
            }
            else
            {
                userData.Coins -= item.Reward;
            }
            Coins = userData.Coins.ToString();
            item = null;
            await UserDataStore.UpdateItemAsync(tempId, userData);
        }


        public void ExecuteCheckedTest(TodoItem item)
        {
            if (IsBusy || item == null) return;
            Console.WriteLine("EXEC CHECKED2 " + item.Text);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            Console.WriteLine("LOAD ITEMS");
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await TodoStore.GetItemsAsync(true);

                foreach (var item in items)
                {
                    Items.Add(item);
                }
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task OnAppearing()
        {
            IsBusy = true;

            SelectedItem = null;

            UserData userData = await GetUserDataAsync();

            Coins = userData.Coins.ToString();

        }
        public Task OnDisappearing()
        {
            IsBusy = true;
            return Task.CompletedTask;
        }

        public TodoItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                //OnItemSelected(value);
            }
        }

        public string Coins
        {
            get => coins;
            set => SetProperty(ref coins, value);
            //LoadUserData();
        }

        public async Task<UserData> GetUserDataAsync()
        {
            UserData data = await UserDataStore.GetItemAsync(tempId);
            int x = 0;
            x++;

            return data;
        }



        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewTodoPage));
        }

        /*async void OnItemSelected(TodoItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            Console.WriteLine(item.Id);
            await Shell.Current.GoToAsync($"{nameof(TodoDetailPage)}?{nameof(TodoDetailViewModel.ItemId)}={item.Id}");
        }*/
    }
}
