using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;
using XamarinTodoApp.Resources;


using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinTodoApp.Services;
using XamarinTodoApp.ViewModels.Todo;
//using XamarinTodoApp.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using XamarinTodoApp.Views.Todo;
using Google.Cloud.Firestore;
using System.Linq;
using System.Reflection;
using Syncfusion.XForms.PopupLayout;
using XamarinTodoApp.Views.Attribute;
using XamarinTodoApp.Helpers;


namespace XamarinTodoApp.ViewModels.Shop
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ShopDetailViewModel : NewRewardBaseViewModel
    {



        private string itemId;
        private RewardItem Item { get; set; }

        private UserData User { get; set; }
        public ShopDetailViewModel()
        {

        }

        public override async void OnSave()
        {
            base.OnSave();

           

            RewardItem newItem = Item;
            newItem.Text = Text;
            newItem.Description = Description;
            newItem.Price = int.Parse(Price);
            newItem.ImagePath = SelectedPath;

            await RewardStore.UpdateItemAsync(ItemId, newItem);
            await Shell.Current.GoToAsync("..");
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
                var item = await RewardStore.GetItemAsync(itemId);
                Item = item;



                Text = item.Text;
                Description = item.Description;
                Price = item.Price.ToString();
                SelectedPath = item.ImagePath;
                SelectedImage = ImageSource.FromResource(SelectedPath, typeof(NewAttributePage).GetTypeInfo().Assembly);


            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
            IsBusy = false;
        }


    }
}
