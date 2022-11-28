using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Views;
using XamarinTodoApp.Models;
using Firebase.Auth;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XamarinTodoApp.Resources;
using Xamarin.Essentials;
using Xamarin.CommunityToolkit.ObjectModel;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using XamarinTodoApp.Views.General;

namespace XamarinTodoApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {


        private string loginEmail;
        private string loginPass;
        public Command LoginCommand { get; }
        public AsyncCommand SignupCommand { get; }
        public FirebaseAuthProvider AuthProvider { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            
            SignupCommand = new AsyncCommand(OnSignupClicked);
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.WebApiKey));
        }

        private List<StatAttribute> CreateAttributes()
        {
            return new List<StatAttribute>
                {
                    new StatAttribute()
                    {
                        Name = "STRENGTH",
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1,
                        ImagePath = "XamarinTodoApp.Resources.AttributeIcons.gym.png"
                    },
                    new StatAttribute()
                    {
                        Name = "INTELLIGENCE",
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1,
                        ImagePath = "XamarinTodoApp.Resources.AttributeIcons.3330916.png"
                    },
                    new StatAttribute()
                    {
                        Name = "SOCIAL",
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1,
                        ImagePath = "XamarinTodoApp.Resources.AttributeIcons.2228649.png"
                    },
                    new StatAttribute()
                    {
                        Name = "CREATIVITY",
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1,
                        ImagePath = "XamarinTodoApp.Resources.AttributeIcons.877637.png"
                    }
                };
        }


        async Task OnSignupClicked()
        {
            //await Shell.Current.GoToAsync("//AboutPage");
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        private async void OnLoginClicked(object obj)
        {

            EmailHasError = false;
            PassHasError = false;
            EmailErrorMessage = "Invalid email";
            PassErrorMessage = "Invalid password";


            if (string.IsNullOrEmpty(LoginEmailEntry))
            {
                EmailHasError = true;
                EmailErrorMessage = "Enter an email";
            }
            else if (!new EmailAddressAttribute().IsValid(LoginEmailEntry))
            {
                EmailHasError = true;
                EmailErrorMessage = "Enter a valid email";
            }

            if (LoginPasswordEntry.Length < 6)
            {
                PassHasError = true;
                PassErrorMessage = "Enter at least 6 characters";
            }

            if (PassHasError || EmailHasError)
                return;

            IsBusy = true;
            var exception = await AuthService.Login(LoginEmailEntry, LoginPasswordEntry);
            if (exception != null)
            {

                if (exception is FirebaseAuthException)
                {
                    var authException = exception as FirebaseAuthException;
                    var reason = authException.Reason;
                    TreatExceptions(reason);
                }

                IsBusy = false;
                return;
            }

            IsBusy = false;

            Application.Current.MainPage = new AppShell();
            // await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
        private void TreatExceptions(AuthErrorReason reason)
        {
            switch (reason)
            {

                case AuthErrorReason.UserNotFound:
                    EmailHasError = true;
                    PassErrorMessage = "User not found";
                    break;

                case AuthErrorReason.WrongPassword:
                    PassHasError = true;
                    PassErrorMessage = "Invalid password";
                    break;

                case AuthErrorReason.MissingPassword:
                    PassHasError = true;
                    PassErrorMessage = "Invalid password";
                    break;

                case AuthErrorReason.WeakPassword:
                    PassHasError = true;
                    PassErrorMessage = "Password is too weak";
                    break;

                case AuthErrorReason.InvalidEmailAddress:
                    EmailHasError = true;
                    EmailErrorMessage = "Enter a valid email";
                    break;

                case AuthErrorReason.EmailExists:
                    EmailHasError = true;
                    EmailErrorMessage = "Email is taken";
                    break;

                case AuthErrorReason.MissingEmail:
                    EmailHasError = true;
                    EmailErrorMessage = "Enter a valid email";
                    break;

                case AuthErrorReason.UnknownEmailAddress:
                    EmailHasError = true;
                    EmailErrorMessage = "Unknown email";
                    break;
                case AuthErrorReason.TooManyAttemptsTryLater:
                    EmailHasError = true;
                    PassHasError = true;
                    EmailErrorMessage = "Too many attempts! Try again later";
                    PassErrorMessage = "Too many attempts! Try again later";
                    break;


                default:
                    PassHasError = true;
                    EmailHasError = true;
                    PassErrorMessage = "Invalid email or password!";
                    break;
            }
        }

        public async Task OnAppearing()
        {
            ResetEntries();
        }

        private void ResetEntries()
        {
            LoginPasswordEntry = "";
            LoginEmailEntry = "";
            EmailHasError = false;
            PassHasError = false;
            EmailErrorMessage = "Invalid email";
            PassErrorMessage = "Invalid password";
        }

        

        private string passErrorMessage;
        public string PassErrorMessage
        {
            get => passErrorMessage;
            set => SetProperty(ref passErrorMessage, value);
        }


        private string emailErrorMessage;
        public string EmailErrorMessage
        {
            get => emailErrorMessage;
            set => SetProperty(ref emailErrorMessage, value);
        }

        private bool emailHasError;
        public bool EmailHasError
        {
            get => emailHasError;
            set => SetProperty(ref emailHasError, value);
        }

        private bool passHasError;
        public bool PassHasError
        {
            get => passHasError;
            set => SetProperty(ref passHasError, value);
        }


        public string LoginEmailEntry
        {
            get => loginEmail;
            set
            {
                EmailHasError = false;
                SetProperty(ref loginEmail, value);
            }
        }


        public string LoginPasswordEntry
        {
            get => loginPass;
            set
            {
                PassHasError = false;
                SetProperty(ref loginPass, value);
            }
        }


    }
}
