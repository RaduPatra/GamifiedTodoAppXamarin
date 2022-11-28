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
using System.Reflection;
using Syncfusion.XForms.PopupLayout;
using XamarinTodoApp.Views.Shop;
using XamarinTodoApp.Helpers;

namespace XamarinTodoApp.ViewModels.Shop
{
    public class NewRewardViewModel : NewRewardBaseViewModel
    {
        
        public NewRewardViewModel()
        {
           

        }

     
        public override async void OnSave()
        {
            DateTime date = DateTime.Now.ToUniversalTime();
            RewardItem newItem = new RewardItem()
            {
                Id = "",
                Text = Text,
                Description = Description,
                Price = int.Parse(Price),
                CreationDate = Timestamp.FromDateTime(date),
                ImagePath = SelectedPath
            };

            await RewardStore.AddRewardAsync(newItem, AuthService.UserInfo.LocalId);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

    }

    
}
