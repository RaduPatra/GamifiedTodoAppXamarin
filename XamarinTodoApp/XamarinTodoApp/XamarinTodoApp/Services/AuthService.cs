using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using XamarinTodoApp.Resources;
using XamarinTodoApp.Models;
using Xamarin.Forms;
using XamarinTodoApp.ViewModels.General;

namespace XamarinTodoApp.Services
{
    public class AuthService : IAuthService
    {
        private FirebaseAuthProvider AuthProvider { get; set; }

        public Firebase.Auth.User UserInfo
        {
            get; set;
        }
        public AuthService()
        {
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.WebApiKey));
        }

        public async Task<Exception> CreateUser(string email, string password)
        {
            try
            {
                var auth = await AuthProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                UserInfo = auth.User;
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex;
            }
        }

        async Task InitUserData()
        {
            var userDataVM = Application.Current.Resources["userDataVM"] as UserDataViewModel;
            await userDataVM.Init();

        }

        public async Task<Exception> Login(string email, string password)
        {
            try
            {
                
                var auth = await AuthProvider.SignInWithEmailAndPasswordAsync(email, password);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set(Constants.FirebaseRefreshToken, serializedContent);
                UserInfo = auth.User;
                await InitUserData();
                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex;

            }

        }


        public async Task RefreshUserToken()
        {
            try
            {
                var savedAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get(Constants.FirebaseRefreshToken, ""));
                if (savedAuth.IsExpired())
                {
                    var refreshedContent = await AuthProvider.RefreshAuthAsync(savedAuth);
                    var serializedContent = JsonConvert.SerializeObject(refreshedContent);
                    Preferences.Set(Constants.FirebaseRefreshToken, serializedContent);
                }

                UserInfo = savedAuth.User;

                await InitUserData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }



    }
}
