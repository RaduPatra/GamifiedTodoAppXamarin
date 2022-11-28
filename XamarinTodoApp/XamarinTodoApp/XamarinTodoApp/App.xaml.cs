using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.Services.Firestore;
using XamarinTodoApp.Views;
using Google.Cloud.Firestore;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using XamarinTodoApp.ViewModels.General;
using XamarinTodoApp.Resources;
using XamarinTodoApp.Views.General;
using Plugin.LocalNotification;
using XamarinTodoApp.Services.Interfaces;

namespace XamarinTodoApp
{
    public partial class App : Application
    {
        //FirestoreDb db;
        AuthService AuthService => DependencyService.Get<AuthService>();
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTEzNzgzQDMxMzkyZTMzMmUzMFRyZDE3MmJMZy9hQmdnMm9WUHc5WUV0ZGFENFU0TVZ6UlhkcmVTK1JYNWs9");
            InitializeComponent();


            //Task.Run(async () => await SetupFirebaseCredentials());
            //DependencyService.Register<MockDataStore>();

            //DependencyService.Register<TodoItemStoreFB>();
            //DependencyService.Register<UserDataFB>();

            DependencyService.Register<ItemStoreFB>();
            DependencyService.Register<FirestoreTodo>();
            DependencyService.Register<FirestoreUserData>();
            DependencyService.Register<FirestoreReward>();

            DependencyService.Register<FirestoreInventory>();


            DependencyService.Register<AuthService>();


            DependencyService.RegisterSingleton<IApplicationCache>(new ApplicationCache());


            DependencyService.Register<FirestoreUser>();//





            var env = DependencyService.Get<IEnvironment>();

            var color = Color.FromHex("#2196F3");
            env?.SetNavigationBarColor(color);
            MainPage = new LoadingPage();


            //NotificationCenter.Current.CancelAll();
        }


        //unit test
        public static string GetFromResources(string resourceName)
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            using (Stream stream = assem.GetManifestResourceStream($"XamarinTodoApp.{resourceName}"))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        //unit test
        public static IEnumerable<string> GetFilenamesFromFolder()
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            var folderName = "XamarinTodoApp.Resources.RewardIcons";
            var fileNames = assem.GetManifestResourceNames().Where(r => r.StartsWith(folderName) && r.EndsWith(".png"));

            foreach (var file in fileNames)
            {
                Console.WriteLine(file);
            }
            return fileNames;
        }


        public async Task SetupFirebaseCredentials()
        {
            const string localFileName = "credentialsLocal.json";
            const string assetFileName = "credentials.json";
            string localPath = Path.Combine(FileSystem.AppDataDirectory, localFileName);

            if (!File.Exists(localPath))
            {
                using (var assetStream = await FileSystem.OpenAppPackageFileAsync(assetFileName))
                {
                    using (FileStream localStream = File.Open(localPath, FileMode.OpenOrCreate))
                    {
                        await assetStream.CopyToAsync(localStream);
                    }
                }
            }

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", localPath);

            //var fileContent = File.ReadAllText(localPath);
        }


        protected override async void OnStart()
        {

            await SetupFirebaseCredentials();
            if (!string.IsNullOrEmpty(Preferences.Get(Constants.FirebaseRefreshToken, "")))
            {

                await AuthService.RefreshUserToken();
                MainPage = new AppShell();
            }
            else
            {
                //MainPage = new LoginPage();
                MainPage = new LoginShell();
            }



        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


    }
}
