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
using XamarinTodoApp.Views.Shop;

namespace XamarinTodoApp.ViewModels.Shop
{
    public class RewardInventoryViewModel : ViewModelBase
    {
        //private Item _selectedItem;
        private string coins;
        private string quantity;
        public ObservableCollection<InventoryItemViewModel> Items { get; set; }
        public AsyncCommand LoadItemsCommand { get; }
        public Command<InventoryItemViewModel> UseItemTapped { get; }


        public Command TestCommand { get; }

        public RewardInventoryViewModel()
        {
            IsBusy = true;
            Title = "Shop";
            Items = new ObservableCollection<InventoryItemViewModel>();
            LoadItemsCommand = new AsyncCommand(ExecuteLoadItemsCommand);

            UseItemTapped = new Command<InventoryItemViewModel>(OnItemUseSelected);

            TestCommand = new Command(TestCom);
        }
        void TestCom() { return; }


        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                //var items = await InventoryStore.GetItemsAsync(true);

                var myItems = await InventoryStore.GetItemsAsync(true);
                //var myItems = items.Where(item => item.CreatedBy.Id == AuthService.UserInfo.LocalId);

                var InvItemVMs = myItems.Select(x => new InventoryItemViewModel(x));

                foreach (var item in InvItemVMs)
                {
                    Items.Add(item);
                }
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
        public async Task<UserData> GetUserDataAsync()
        {
            //UserData data = await UserDataStore.GetItemAsync(tempId);
            UserData data = await UserDataStore.GetItemAsync(AuthService.UserInfo.LocalId);
            return data;
        }

        public async Task OnAppearing()
        {
            IsBusy = true;

            //SelectedItem = null;
        }

        /* public RewardItem SelectedItem
         {
             get => _selectedItem;
             set
             {
                 SetProperty(ref _selectedItem, value);
                 OnItemSelected(value);
             }
         }*/


        async void OnItemUseSelected(InventoryItemViewModel itemVM)
        {
            Console.WriteLine("used " + itemVM.Item.RewardItem.Text);
            itemVM.Item.Quantity--;
            itemVM.Quantity = itemVM.Item.Quantity.ToString();


            await InventoryStore.UpdateItemAsync(itemVM.Item.Id, itemVM.Item);
            if (itemVM.Item.Quantity <= 0)
            {
                Items.Remove(itemVM);
                await InventoryStore.DeleteItemAsync(itemVM.Item.Id);
            }

        }
    }
}
