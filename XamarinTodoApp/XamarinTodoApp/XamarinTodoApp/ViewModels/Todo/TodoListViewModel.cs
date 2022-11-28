//using MvvmHelpers.Commands;
using Google.Cloud.Firestore;
using Plugin.LocalNotification;
using System;

//using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

//using XamarinTodoApp.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinTodoApp.Helpers;
using XamarinTodoApp.Models;
using XamarinTodoApp.Resources;
using XamarinTodoApp.Services;
using XamarinTodoApp.Services.Interfaces;
using XamarinTodoApp.ViewModels.General;
using XamarinTodoApp.ViewModels.Todo;
using XamarinTodoApp.Views.Todo;

namespace XamarinTodoApp.ViewModels
{
    public class TodoListViewModel : ViewModelBase, IContextMenu<TodoItemViewModel>
    {
        public ObservableCollection<TodoGroup> itemsGroup;

        private readonly string coins;

        private readonly string level;

        //private readonly Action<TodoItemViewModel, bool> updateTodoAction;
        private readonly CustomAction updateTodoAction;

        private TodoItemViewModel _selectedItem;
        private ContextMenuViewModel<TodoItemViewModel> contextMenuVM;
        private bool isLoading;
        private bool isNavBarOn;
        private int itemsSelectedCount;
        private Command<TodoItemViewModel> itemTapped;

        public TodoListViewModel()
        {
            //IsBusy = true;
            IsLoading = false;
            IsNavBarOn = true;
            Title = "Todo browse";
            Items = new ObservableCollection<TodoItemViewModel>();
            ItemsGroup = new ObservableCollection<TodoGroup>();
            updateTodoAction = async (a, b, c) => await UpdateCheckedAsync(a, b, c);

            LoadItemsCommand = new AsyncCommand(ExecuteLoadItemsCommand);
            ItemTapped = new Command<TodoItemViewModel>(OnItemSelected);//go to detail page
            AddItemCommand = new Command(OnAddItem);
            DeleteItemCommand = new AsyncCommand<TodoItemViewModel>(DeleteItem);
            SortAlphabeticallyCommand = new Command(SortAlphabetically);
            PageAppearingCommand = new AsyncCommand(OnAppearing);

            TestCommand = new Command(DebugCommand);

            ContextMenuVM = new ContextMenuViewModel<TodoItemViewModel>(value => ItemTapped = value, DeleteItem, OnItemSelected);

            TimerSetup();

        }

        public Command AddItemCommand { get; }
        public Command CheckedCommand { get; }
        public Command<Frame> CloseContextMenuCommand { get; }
        public Command<object> ContextMenuCommand { get; }
        public ContextMenuViewModel<TodoItemViewModel> ContextMenuVM
        {
            get => contextMenuVM;
            set => SetProperty(ref contextMenuVM, value);
        }

        public AsyncCommand<TodoItemViewModel> DeleteItemCommand { get; }
        public AsyncCommand DeleteSelectedCommand { get; }

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public bool IsNavBarOn
        {
            get => isNavBarOn;
            set => SetProperty(ref isNavBarOn, value);
        }

        public ObservableCollection<TodoItemViewModel> Items { get; set; }
        public ObservableCollection<TodoGroup> ItemsGroup { get => itemsGroup; set => SetProperty(ref itemsGroup, value); }

        public int ItemsSelectedCount
        {
            get => itemsSelectedCount;
            set
            {
                SetProperty(ref itemsSelectedCount, value);
            }
        }

        public Command<TodoItemViewModel> ItemTapped { get => itemTapped; set => SetProperty(ref itemTapped, value); }
        public AsyncCommand LoadItemsCommand { get; }
        public Command<object> LongPressCommand { get; }
        public AsyncCommand PageAppearingCommand { get; }

        public TodoItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                //OnItemSelected(value);
            }
        }

        public Command SortAlphabeticallyCommand { get; }
        public Command<TodoItemViewModel> TapSelectCommand { get; }
        public Command TestCommand { get; }
        public UserData UserData { get; set; }

        public async Task<UserData> GetUserDataAsync()
        {
            //UserData data = await UserDataStore.GetItemAsync(tempId);
            UserData data = await UserDataStore.GetItemAsync(AuthService.UserInfo.LocalId);
            return data;
        }

        public async Task OnAppearing()
        {
            Console.WriteLine("ON APPEARING");
            IsBusy = true;

            SelectedItem = null;
            IsNavBarOn = true;
            ContextMenuVM.OnAppearing();

            if (ApplicationCache.UserData == null)
            {
                ApplicationCache.UserData = await GetUserDataAsync();
            }
            UserData = ApplicationCache.UserData;
        }

        public Task OnDisappearing()
        {
            Console.WriteLine("ON DISAPPEARING");
            // IsBusy = true;
            return Task.CompletedTask;
        }

        private DateTime? CalculateNextDate(TodoItem item)
        {
            if (!item.DueDate.HasValue || item.RepeatFrequency == 0) return null;

            var todoTimestamp = item.DueDate.Value;

            var todoDueLocalDate = todoTimestamp.ToDateTime().ToLocalTime();
            var repeatFrequencySeconds = item.RepeatFrequency * 24 * 60 * 60;
            int secondsToAdd = (((int)(DateTime.Now - todoDueLocalDate).TotalSeconds / repeatFrequencySeconds) + 1) * repeatFrequencySeconds;

            var nextDate = todoDueLocalDate.AddSeconds(secondsToAdd);
            return nextDate;
        }

        private async Task CreateGroups()
        {
            ItemsGroup.Clear();
            var groupItems = await TodoStore.GetItemsAsync(true);

            //var myTodos = groupItems.Where(item => item.CreatedBy.Id == AuthService.UserInfo.LocalId).Where(item => item.IsDone == false);
            //var myCompletedTodos = groupItems.Where(item => item.CreatedBy.Id == AuthService.UserInfo.LocalId).Where(item => item.IsDone == true);
            var myTodos = groupItems.Where(item => item.IsDone == false);
            var myCompletedTodos = groupItems.Where(item => item.IsDone == true);

            var todoItemVMs = myTodos.Select(x => new TodoItemViewModel(x, updateTodoAction));
            var completedTodoItemVMs = myCompletedTodos.Select(x => new TodoItemViewModel(x, updateTodoAction));

            var todoGroup = new ObservableCollection<TodoItemViewModel>(todoItemVMs);
            var completedGroup = new ObservableCollection<TodoItemViewModel>(completedTodoItemVMs);

            if (todoItemVMs.Any())
                ItemsGroup.Add(new TodoGroup("Todos", todoGroup));
            else ItemsGroup.Add(new TodoGroup("Todos", new ObservableCollection<TodoItemViewModel>()));

            if (completedTodoItemVMs.Any())
                ItemsGroup.Add(new TodoGroup("Completed", completedGroup));
            else ItemsGroup.Add(new TodoGroup("Completed", new ObservableCollection<TodoItemViewModel>()));
        }

        private void DebugCommand()
        {
            //Shell.Current.FlyoutIsPresented = true;   //close the menu
            //NotificationCenter.Current.CancelAll();
        }

        private async Task DeleteItem(TodoItemViewModel itemVM)
        {
            Console.WriteLine("delete called");
            IsLoading = true;
            try
            {
                //0 - todos, 1 - completed
                if (itemVM.IsChecked)
                {
                    ItemsGroup[1].Remove(itemVM);
                }
                else
                {
                    ItemsGroup[0].Remove(itemVM);
                }

                Items.Remove(itemVM);

                NotificationCenter.Current.Cancel(itemVM.Item.NotificationId);

                await TodoStore.DeleteItemAsync(itemVM.Item.Id);
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

        private async Task ExecuteLoadItemsCommand()
        {
            Console.WriteLine("LOAD ITEMS");
            IsBusy = true;
            ContextMenuVM.OnAppearing();
            try
            {
                await CreateGroups();
                //await RepeatCompletedTodo();

                Console.WriteLine("exec changed");

                /*var items = await TodoStore.GetItemsAsync(true);
                var myItems = items.Where(item => item.CreatedBy.Id == AuthService.UserInfo.LocalId);
                myItems = myItems.OrderByDescending(x => x.CreationDate);
                var todoItemVMs = myItems.Select(x => new TodoItemViewModel(x, updateTodoAction));
                UpdateCollection(todoItemVMs);*/
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

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewTodoPage));
        }

        private async void OnItemSelected(TodoItemViewModel itemVM)
        {
            if (itemVM == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            Console.WriteLine(itemVM.Item.Id);
            await Shell.Current.GoToAsync($"{nameof(TodoDetailPage)}?{nameof(TodoDetailViewModel.ItemId)}={itemVM.Item.Id}");
        }

        private void RefreshUserData(UserData userData)
        {
            ApplicationCache.UserData = userData;
            UserData = userData;
        }

       
        //orderby selected type w reflection- https://stackoverflow.com/questions/33159266/a-replacement-for-orderby-switch-in-c-sharp
        private void SortAlphabetically()
        {
            IsLoading = true;
            var sortedItems = Items.OrderBy(x => x.Item.Text).ToList();
            UpdateCollection(sortedItems);

            var todoGroup = ItemsGroup[0].OrderBy(x => x.Item.Text).ToList();
            var completedGroup = ItemsGroup[1].OrderBy(x => x.Item.Text).ToList();

            for (int i = 0; i < todoGroup.Count; i++)
            {
                itemsGroup[0].Move(itemsGroup[0].IndexOf(todoGroup[i]), i);
            }

            for (int i = 0; i < completedGroup.Count; i++)
            {
                itemsGroup[1].Move(itemsGroup[1].IndexOf(completedGroup[i]), i);
            }

            IsLoading = false;
        }

        private void TimerSetup()
        {
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                // do something every X seconds
                var ItemsToUpdate = new List<TodoItemViewModel>();
                if (ItemsGroup != null)
                {
                    foreach (var group in ItemsGroup)
                    {
                        foreach (var itemVM in group)
                        {
                            if (!itemVM.Item.DueDate.HasValue) continue;

                            var dueDate = itemVM.Item.DueDate.Value.ToDateTime().ToLocalTime();
                            bool nextDeadlineStarted = DateTime.Now > dueDate;

                            if (nextDeadlineStarted && itemVM.Item.RepeatFrequency != 0)
                            {
                                if (group.GroupName == "Todos")
                                    itemVM.ItemDueDate = Timestamp.FromDateTime(dueDate.AddMilliseconds(1).ToUniversalTime());
                                else ItemsToUpdate.Add(itemVM);
                            }
                        }
                    }
                }

                foreach (var itemVM in ItemsToUpdate)
                {
                    itemVM.IsCheckedAuto = false;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    // interact with UI elements
                });
                return true; // runs again, or false to stop
            });
        }

        private async Task UpdateCheckedAsync(TodoItemViewModel itemVM, bool isChecked, bool autoUpdate = false)
        {
            if (itemVM == null || IsLoading) return;
            IsLoading = true;
            try
            {
                //update item with new check value
                itemVM.Item.IsDone = isChecked;
                //await TodoStore.UpdateItemAsync(item.Item.Id, item.Item);

                //update userData coins/xp
                var xpToChange = 0;
                var coinsToChange = 0;

                if (itemVM.Item.IsDone)
                {
                    xpToChange = itemVM.Item.ExpReward;
                    coinsToChange = itemVM.Item.Reward;
                    var isCheckedAfterDeadline = itemVM.Item.DueDate.HasValue && DateTime.Now > itemVM.Item.DueDate.Value.ToDateTime().ToLocalTime();
                    if (isCheckedAfterDeadline && itemVM.Item.RepeatFrequency != 0)
                    {
                        //dont move to completed when checking a due item, start next automatically
                        itemVM.IsCheckedVisual = false;
                        itemVM.Item.IsDone = false;

                        var nextDate = CalculateNextDate(itemVM.Item);
                        if (nextDate.HasValue)
                        {
                            var nextDueDateTimestamp = Timestamp.FromDateTime((nextDate.Value).ToUniversalTime());
                            itemVM.Item.DueDate = nextDueDateTimestamp;
                        }
                    }
                    else
                    {
                        ItemsGroup[0].Remove(itemVM);
                        ItemsGroup[1].Add(itemVM);
                    }
                }
                else
                {
                    if (autoUpdate)
                    {
                        //calc next deadline when moving back completed item
                        var nextDate = CalculateNextDate(itemVM.Item);
                        if (nextDate.HasValue)
                        {
                            var nextDueDateTimestamp = Timestamp.FromDateTime((nextDate.Value).ToUniversalTime());
                            itemVM.Item.DueDate = nextDueDateTimestamp;
                        }
                    }
                    else
                    {
                        xpToChange = -itemVM.Item.ExpReward;
                        coinsToChange = -itemVM.Item.Reward;
                    }
                    ItemsGroup[1].Remove(itemVM);
                    ItemsGroup[0].Add(itemVM);
                }
                await TodoStore.UpdateItemAsync(itemVM.Item.Id, itemVM.Item);
                var payloadXP = new PayloadXP()
                {
                    ExpAmount = xpToChange,
                    TodoAttributes = itemVM.Item.StatAttributes,
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

        private void UpdateCollection(IEnumerable<TodoItemViewModel> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }
}