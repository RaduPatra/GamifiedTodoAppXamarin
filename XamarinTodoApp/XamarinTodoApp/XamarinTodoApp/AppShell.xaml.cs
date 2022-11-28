using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;
using XamarinTodoApp.ViewModels;
using XamarinTodoApp.Resources;
using XamarinTodoApp.Views;
using XamarinTodoApp.Views.Todo;
using XamarinTodoApp.Views.Shop;
using XamarinTodoApp.Services;
using System.Collections.ObjectModel;
using XamarinTodoApp.Views.Attribute;
using XamarinTodoApp.Views.General;

namespace XamarinTodoApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        AuthService AuthService => DependencyService.Get<AuthService>();


        public AppShell()
        {
            InitializeComponent();
            //items
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            //todos
            Routing.RegisterRoute(nameof(TodoDetailPage), typeof(TodoDetailPage));
            Routing.RegisterRoute(nameof(NewTodoPage), typeof(NewTodoPage));

            //shop
            Routing.RegisterRoute(nameof(NewRewardPage), typeof(NewRewardPage));

            Routing.RegisterRoute(nameof(NewAttributePage), typeof(NewAttributePage));

            Routing.RegisterRoute(nameof(AttributePage), typeof(AttributePage));
            Routing.RegisterRoute(nameof(TodoListPage), typeof(TodoListPage));


            Routing.RegisterRoute(nameof(LoginShell), typeof(LoginShell));
            Routing.RegisterRoute(nameof(UpdateAttributePage), typeof(UpdateAttributePage));
            Routing.RegisterRoute(nameof(RewardDetailPage), typeof(RewardDetailPage));

            //((NavigationPage)Application.Current.MainPage).navi = Color.Red;

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Preferences.Remove(Constants.FirebaseRefreshToken);
            Shell.Current.FlyoutIsPresented = false;
            //await Shell.Current.GoToAsync("//LoginShell");
            //await Shell.Current.GoToAsync("//LoginPage");
            App.Current.MainPage = new LoginShell();
        }

        private async void OnMenuItemClicked2(object sender, EventArgs e)
        {
            Preferences.Remove(Constants.FirebaseRefreshToken);
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync("AttributePage");
        }

    }
}
 