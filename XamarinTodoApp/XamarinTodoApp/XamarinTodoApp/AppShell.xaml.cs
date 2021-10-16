using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinTodoApp.ViewModels;
using XamarinTodoApp.Views;
using XamarinTodoApp.Views.Todo;

namespace XamarinTodoApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            //items
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            //todos
            Routing.RegisterRoute(nameof(TodoDetailPage), typeof(TodoDetailPage));
            Routing.RegisterRoute(nameof(NewTodoPage), typeof(NewTodoPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
