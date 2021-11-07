
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
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Linq;
using XamarinTodoApp.ViewModels.General;
using XamarinTodoApp.Resources;
using XamarinTodoApp.Helpers;

namespace XamarinTodoApp.ViewModels
{
    public class TodoListViewModel : ViewModelBase
    {
        private readonly Action<TodoItem, bool> updateTodoAction;
        private TodoItemViewModel _selectedItem;
        private string coins;
        private bool isLoading;
        private string level;
        public UserData UserData { get; set; }

        //?
        public UserDataViewModel UserDataViewModel { get; set; }
        public ObservableCollection<TodoItemViewModel> Items { get; set; }
        public AsyncCommand LoadItemsCommand { get; }
        public Command CheckedCommand { get; }
        public Command AddItemCommand { get; }
        public Command<TodoItemViewModel> ItemTapped { get; }
        public AsyncCommand<TodoItemViewModel> DeleteItemCommand { get; }

        public Command SortAlphabeticallyCommand { get; }
        public Command TestCommand { get; }


        public TodoListViewModel()
        {
            IsBusy = true;
            IsLoading = false;
            Title = "Todo browse";
            Items = new ObservableCollection<TodoItemViewModel>();
            updateTodoAction = async (a, b) => await UpdateCheckedAsync(a, b);

            LoadItemsCommand = new AsyncCommand(ExecuteLoadItemsCommand);
            ItemTapped = new Command<TodoItemViewModel>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
            TestCommand = new Command(DebugCommand);
            DeleteItemCommand = new AsyncCommand<TodoItemViewModel>(DeleteItem);
            SortAlphabeticallyCommand = new Command(SortAlphabetically);
            //CheckedCommand = new Command<TodoItemViewModel>(async (item) => await ExecuteChecked(item));
        }


        void DebugCommand()
        {

        }


        //orderby selected type w reflection- https://stackoverflow.com/questions/33159266/a-replacement-for-orderby-switch-in-c-sharp
        void SortAlphabetically()
        {
            IsLoading = true;
            var sortedItems = Items.OrderBy(x => x.Item.Text).ToList();
            UpdateCollection(sortedItems);
            IsLoading = false;
        }

        private void UpdateCollection(IEnumerable<TodoItemViewModel> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async Task ExecuteLoadItemsCommand()
        {

            Console.WriteLine("LOAD ITEMS");
            IsBusy = true;
            try
            {
                var items = await TodoStore.GetItemsAsync(true);
                items = items.OrderByDescending(x => x.CreationDate);
                var todoItemVMs = items.Select(x => new TodoItemViewModel(x, updateTodoAction));
                UpdateCollection(todoItemVMs);
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

        private async Task UpdateCheckedAsync(TodoItem item, bool isChecked)
        {
            if (item == null || IsLoading) return;
            IsLoading = true;
            try
            {
                //update item with new check value
                item.IsDone = isChecked;
                await TodoStore.UpdateItemAsync(item.Id, item);

                //update userData coins/xp
                var xpToChange = 0;
                var coinsToChange = 0;
                if (item.IsDone)
                {
                    xpToChange = item.ExpReward;
                    coinsToChange = item.Reward;
                }
                else
                {
                    xpToChange = -item.ExpReward;
                    coinsToChange = -item.Reward;
                }

                var payloadXP = new PayloadXP()
                {
                    ExpAmount = xpToChange,
                    Callback = RefreshUserData
                };

                var payloadCoins = new PayloadCoins()
                {
                    CoinsAmount = coinsToChange,
                    Callback = RefreshUserData
                };
                MessagingCenter.Send<ViewModelBase, PayloadXP>(this, MessageChannel.UserXPChanged, payloadXP);
                MessagingCenter.Send<ViewModelBase, PayloadCoins>(this, MessageChannel.UserCoinsChanged, payloadCoins);


                //Coins = UserData.Coins.ToString();
                //await UserDataStore.UpdateItemAsync(tempId, UserData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }


        void RefreshUserData(UserData userData)
        {
            ApplicationCache.UserData = userData;
            UserData = userData;
        }
        /*private void AddExperience(UserData userData)
        {
            userData.LevelData.CurrentExperience++;
        }*/

        private async Task DeleteItem(TodoItemViewModel item)
        {
            Console.WriteLine("delete called");
            IsLoading = true;
            try
            {
                Items.Remove(item);
                await TodoStore.DeleteItemAsync(item.Item.Id);
                //await Task.Delay(1000);
                // Items.Remove(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }


        public void ExecuteCheckedTest(TodoItemViewModel item)
        {
            if (IsBusy || item == null) return;
            Console.WriteLine("EXEC CHECKED2 " + item.Item.Text);
        }

        public async Task OnAppearing()
        {
            Console.WriteLine("ON APPEARING");
            IsBusy = true;

            SelectedItem = null;



            if (ApplicationCache.UserData == null)
            {
                ApplicationCache.UserData = await GetUserDataAsync();
            }
            UserData = ApplicationCache.UserData;

            //MessagingCenter.Send<ViewModelBase, Action>(this, "UserDataChanged", TestAction);
            //Coins = UserData.Coins.ToString();

            //UserDataViewModel.Level = UserData.LevelData.Level.ToString();

        }

        void TestAction()
        {

        }
        public Task OnDisappearing()
        {
            Console.WriteLine("ON DISAPPEARING");
            IsBusy = true;
            return Task.CompletedTask;
        }

        public TodoItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                //OnItemSelected(value);
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public async Task<UserData> GetUserDataAsync()
        {
            UserData data = await UserDataStore.GetItemAsync(tempId);
            return data;
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewTodoPage));
        }

        async void OnItemSelected(TodoItemViewModel item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            Console.WriteLine(item.Item.Id);
            await Shell.Current.GoToAsync($"{nameof(TodoDetailPage)}?{nameof(TodoDetailViewModel.ItemId)}={item.Item.Id}");
        }


    }

}
