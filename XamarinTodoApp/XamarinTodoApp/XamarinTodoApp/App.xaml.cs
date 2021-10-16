using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.Views;

namespace XamarinTodoApp
{
    public partial class App : Application
    {

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTEzNzgzQDMxMzkyZTMzMmUzMFRyZDE3MmJMZy9hQmdnMm9WUHc5WUV0ZGFENFU0TVZ6UlhkcmVTK1JYNWs9");
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            DependencyService.Register<ItemStoreFB>();
            DependencyService.Register<TodoItemStoreFB>();
            DependencyService.Register<UserDataFB>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
