using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.Views;
using XamarinTodoApp.Resources;
using XamarinTodoApp.Views.Attribute;
using XamarinTodoApp.Services.Interfaces;
using XamarinTodoApp.ViewModels.General;

namespace XamarinTodoApp.ViewModels.Attribute
{
    public class AttributeViewModel : ViewModelBase, IContextMenu<AttributeItemViewModel>
    {
        //private Item _selectedItem;
        public AsyncCommand PageAppearingCommand { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public AsyncCommand<AttributeItemViewModel> DeleteItemCommand { get; }


        private Command<AttributeItemViewModel> itemTapped;
        public Command<AttributeItemViewModel> ItemTapped { get => itemTapped; set => SetProperty(ref itemTapped, value); }
        //public Command ItemTapped { get; }



        public Command TestCommand { get; }
        private ContextMenuViewModel<AttributeItemViewModel> contextMenuVM;
        public ContextMenuViewModel<AttributeItemViewModel> ContextMenuVM
        {
            get => contextMenuVM;
            set => SetProperty(ref contextMenuVM, value);
        }

        public AttributeViewModel()
        {
            Title = "Attributes";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            ItemTapped = new Command<AttributeItemViewModel>(OnItemSelected);
            //ItemTapped = new Command(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
            TestCommand = new Command(TestCom);
            DeleteItemCommand = new AsyncCommand<AttributeItemViewModel>(DeleteItem);
            ContextMenuVM = new ContextMenuViewModel<AttributeItemViewModel>(value => ItemTapped = value, DeleteItem, OnItemSelected);
        }
        void TestCom() { return; }

        private async Task DeleteItem(AttributeItemViewModel itemVM)
        {
            Console.WriteLine("delete called");
            IsLoading = true;
            try
            {
                MessagingCenter.Send<ViewModelBase, StatAttribute>(this, MessageChannel.UserRemoveAttribute, itemVM.Item);
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
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                MessagingCenter.Send<ViewModelBase>(this, MessageChannel.RefreshUserData);
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
            ContextMenuVM.OnAppearing();
            //SelectedItem = null;
        }
        public bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }


        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewAttributePage));
        }


        async void OnItemSelected(AttributeItemViewModel itemVM)
        {
            if (itemVM == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            Console.WriteLine("item selected " + itemVM.Item.Id);
            await Shell.Current.GoToAsync($"{nameof(UpdateAttributePage)}?{nameof(AttributeDetailViewModel.ItemId)}={itemVM.Item.Id}");
        }



    }
}
