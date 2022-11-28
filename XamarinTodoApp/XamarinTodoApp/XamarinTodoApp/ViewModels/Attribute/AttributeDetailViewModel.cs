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
namespace XamarinTodoApp.ViewModels.Attribute
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class AttributeDetailViewModel : NewAttributeBaseViewModel
    {
        private string itemId;
        private StatAttribute Item { get; set; }

        private UserData User { get; set; }
        public AttributeDetailViewModel()
        {

        }

        public override async void OnSave()
        {
            base.OnSave();
            


           
            Item.Name = Text;
            Item.ImagePath = SelectedPath;

            MessagingCenter.Send<ViewModelBase, StatAttribute>(this, MessageChannel.UpdateUserAttribute, Item);
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
                var user = await UserDataStore.GetItemAsync(AuthService.UserInfo.LocalId);
                var item = user.StatAttributes.Find(x => x.Id == itemId);


                User = user;
                Item = item;



                Text = item.Name;
                SelectedPath = item.ImagePath;
                SelectedImage = ImageSource.FromResource(SelectedPath, typeof(NewAttributePage).GetTypeInfo().Assembly);

                //var item = await attribute.GetItemAsync(itemId);
                /*Item = item;
                Id = item.Id;
                Text = item.Text;*/

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
            IsBusy = false;
        }



    }
}
