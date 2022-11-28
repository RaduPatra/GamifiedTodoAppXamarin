using Syncfusion.XForms.PopupLayout;
using System;
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
using XamarinTodoApp.Views.Shop;

namespace XamarinTodoApp.ViewModels.Shop
{
    public class RewardShopViewModel : ViewModelBase, IContextMenu<RewardItemViewModel>
    {
        //private Item _selectedItem;
        private string coins;

        private string testval;
        private string quantityEntry;
        private SfPopupLayout currentPopup;
        public UserData UserData { get; set; }

        public ObservableCollection<RewardItemViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; }
        public Command CloseButtonCommand { get; }
        public Command AddItemCommand { get; }
        public Command<RewardItemViewModel> BuyItemTapped { get; }
        public Command<object> OpenPopupCommand { get; set; }
        public AsyncCommand<RewardItemViewModel> DeleteItemCommand { get; }

        public Command<SfPopupLayout> PopupTestCommand { get; set; }
        private Command<RewardItemViewModel> itemTapped;
        public Command<RewardItemViewModel> ItemTapped { get => itemTapped; set => SetProperty(ref itemTapped, value); }

        public Command TestCommand { get; }
        private ContextMenuViewModel<RewardItemViewModel> contextMenuVM;

        public ContextMenuViewModel<RewardItemViewModel> ContextMenuVM
        {
            get => contextMenuVM;
            set => SetProperty(ref contextMenuVM, value);
        }

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
            CloseButtonCommand = new Command(OnClose);

            OpenPopupCommand = new Command<object>(OnOpenPopup);

            PopupTestCommand = new Command<SfPopupLayout>(OnTestPopup);

            ItemTapped = new Command<RewardItemViewModel>(OnItemSelected);
            DeleteItemCommand = new AsyncCommand<RewardItemViewModel>(DeleteItem);

            ContextMenuVM = new ContextMenuViewModel<RewardItemViewModel>(value => ItemTapped = value, DeleteItem, OnItemSelected);
        }

        private void TestCom()
        { return; }

        private void OnTestPopup(SfPopupLayout popup)
        {
            popup.Show();
        }

        private void OnClose()
        {
            CloseCurrentPopup();
        }

        private void OnOpenPopup(object parameter)
        {
            var values = (object[])parameter;
            var popupLayout = values[0] as SfPopupLayout;
            var rewardItemVM = values[1] as RewardItemViewModel;

            currentPopup = popupLayout;
            popupLayout.BindingContext = rewardItemVM;
            QuantityEntry = "1";
            popupLayout.Show();
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                //var items = await RewardStore.GetItemsAsync(true);

                var myItems = await RewardStore.GetItemsAsync(true);
                //var myItems = items.Where(item => item.CreatedBy.Id == AuthService.UserInfo.LocalId);

                var RewardItemVMs = myItems.Select(x => new RewardItemViewModel(x));

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

        private async Task DeleteItem(RewardItemViewModel itemVM)
        {
            Console.WriteLine("delete called");

            try
            {
                await RewardStore.DeleteItemAsync(itemVM.Item.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
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
            if (ApplicationCache.UserData == null)
            {
                ApplicationCache.UserData = await GetUserDataAsync();
            }
            UserData = ApplicationCache.UserData;
            ContextMenuVM.OnAppearing();
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

        private bool hasQuantityError;

        public bool HasQuantityError
        {
            get => hasQuantityError;
            set => SetProperty(ref hasQuantityError, value);
        }

        public string QuantityEntry
        {
            get => quantityEntry;
            set
            {
                HasQuantityError = false;
                string cleanString = Helpers.Helpers.RemoveInvalidNumberInput(value);
                quantityEntry = quantityEntry + "x";
                SetProperty(ref quantityEntry, cleanString);
            }
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

        private async void OnItemBuySelected(RewardItemViewModel item)
        {
            Console.WriteLine(item.Item.Text);
            
            if (QuantityEntry =="0"|| string.IsNullOrEmpty(QuantityEntry))
            {
                HasQuantityError = true;
                return;
            }
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
                    //await InventoryStore.UpdateItemAsync(inventoryItem.Id, inventoryItem);
                    await InventoryStore.UpdateInventoryItemAsync(inventoryItem, inventoryItem.Id, AuthService.UserInfo.LocalId);
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

                    //await InventoryStore.UpdateItemAsync(newInventoryItem.Id, newInventoryItem);
                    await InventoryStore.UpdateInventoryItemAsync(newInventoryItem, newInventoryItem.Id, AuthService.UserInfo.LocalId);
                }
            }
            else
            {
                //activate toast
                Console.WriteLine("not enough money");
            }
        }

        private void RefreshUserData(UserData userData)
        {
            ApplicationCache.UserData = userData;
            UserData = userData;
        }

        private async void OnItemSelected(RewardItemViewModel itemVM)
        {
            if (itemVM == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            Console.WriteLine("item selected " + itemVM.Item.Id);
            await Shell.Current.GoToAsync($"{nameof(RewardDetailPage)}?{nameof(ShopDetailViewModel.ItemId)}={itemVM.Item.Id}");
        }
    }
}