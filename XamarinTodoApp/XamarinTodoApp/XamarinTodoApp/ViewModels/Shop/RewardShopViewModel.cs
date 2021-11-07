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
using Syncfusion.XForms.PopupLayout;
using System.Reflection;
using XamarinTodoApp.Helpers;
using XamarinTodoApp.Resources;

namespace XamarinTodoApp.ViewModels.Shop
{
    public class RewardShopViewModel : ViewModelBase
    {
        //private Item _selectedItem;
        private string coins;
        private string testval;
        private string quantityEntry;
        private SfPopupLayout currentPopup;
        public UserData UserData { get; set; }
      


        public ObservableCollection<RewardItemViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<RewardItemViewModel> BuyItemTapped { get; }
        public Command<object> OpenPopupCommand { get; set; }

        public Command<SfPopupLayout> PopupTestCommand { get; set; }

        


        public Command TestCommand { get; }

        public RewardShopViewModel()
        {
            IsBusy = true;
            Title = "Shop";
            TestProp = "bruh";
            QuantityEntry = "1";
            Items = new ObservableCollection<RewardItemViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            BuyItemTapped = new Command<RewardItemViewModel>(OnItemBuySelected);

            AddItemCommand = new Command(OnAddItem);

            TestCommand = new Command(TestCom);

            OpenPopupCommand = new Command<object>(OnOpenPopup);

            PopupTestCommand = new Command<SfPopupLayout>(OnTestPopup);

        }
        void TestCom() { return; }

        void OnTestPopup(SfPopupLayout popup)
        {
            popup.Show();
        }

        void OnOpenPopup(object parameter)
        {
            var values = (object[])parameter;
            var popupLayout = values[0] as SfPopupLayout;
            var rewardItemVM = values[1] as RewardItemViewModel;

            currentPopup = popupLayout;
            popupLayout.BindingContext = rewardItemVM;
            QuantityEntry = "1";
            popupLayout.Show();
        }
        
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await RewardStore.GetItemsAsync(true);
                var RewardItemVMs = items.Select(x => new RewardItemViewModel(x));

                foreach (var item in RewardItemVMs)
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
            UserData data = await UserDataStore.GetItemAsync(tempId);
            return data;
        }

        public async Task OnAppearing()
        {
            IsBusy = true;
            if (ApplicationCache.UserData == null)
            {
                ApplicationCache.UserData = await GetUserDataAsync();
            }
            UserData = ApplicationCache.UserData;
            //Coins = UserData.Coins.ToString();
            //SelectedItem = null;
        }

        private void CloseCurrentPopup()
        {
            if (currentPopup != null)
            {
                currentPopup.Dismiss();
            }
        }

        public string TestProp
        {
            get => testval;
            set => SetProperty(ref testval, value);
        }

        public string QuantityEntry
        {
            get => quantityEntry;
            set => SetProperty(ref quantityEntry, value);
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

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewRewardPage));
        }


        async void OnItemBuySelected(RewardItemViewModel item)
        {
            Console.WriteLine(item.Item.Text);
            var quantity = int.Parse(QuantityEntry);
            var price = item.Item.Price * quantity;
            if (UserData.Coins - price >= 0)
            {
                CloseCurrentPopup();
                //!
                /*UserData.Coins -= price;
                await UserDataStore.UpdateItemAsync(tempId, UserData);
                Coins = UserData.Coins.ToString();*/
                //!
                var payloadCoins = new PayloadCoins()
                {
                    CoinsAmount = -price,
                    Callback = RefreshUserData
                };
                MessagingCenter.Send<ViewModelBase, PayloadCoins>(this, MessageChannel.UserCoinsChanged, payloadCoins);

                var inventoryItem = await InventoryStore.GetItemAsync(item.Item.Id);
                if (inventoryItem != default)
                {
                    Console.WriteLine("UPDATE INV ITEM");
                    inventoryItem.Quantity += quantity;
                    await InventoryStore.UpdateItemAsync(inventoryItem.Id, inventoryItem);
                }
                else
                {
                    Console.WriteLine("CREATE INV ITEM");
                    InventoryItem newInventoryItem = new InventoryItem()
                    {
                        Id = item.Item.Id,
                        RewardItem = new RewardItem()
                        {
                            Id = item.Item.Id,
                            Text = item.Item.Text,
                            Price = price,
                            CreationDate = item.Item.CreationDate,
                            ImagePath = item.Item.ImagePath
                        },
                        Quantity = quantity
                    };

                    await InventoryStore.UpdateItemAsync(newInventoryItem.Id, newInventoryItem);

                }
            }
            else
            {
                //activate toast
                Console.WriteLine("not enough money");
            }
        }

        void RefreshUserData(UserData userData)
        {
            ApplicationCache.UserData = userData;
            UserData = userData;
        }
        /*async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            Console.WriteLine("item selected " + item.Id);
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }*/
    }
}
