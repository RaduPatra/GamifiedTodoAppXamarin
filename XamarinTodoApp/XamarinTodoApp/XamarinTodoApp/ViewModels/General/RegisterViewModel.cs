using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinTodoApp.Models;
using XamarinTodoApp.Resources;

namespace XamarinTodoApp.ViewModels.General
{
    public class RegisterViewModel : ViewModelBase
    {
        private string emailErrorMessage;
        private bool emailHasError;
        private string passErrorMessage;
        private bool passHasError;
        private string regiserEmail;
        private string registerPass;
        public RegisterViewModel()
        {
            RegisterCommand = new AsyncCommand(OnRegisterClicked);
            AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.WebApiKey));
            PageAppearingCommand = new AsyncCommand(OnAppearing);
        }

        public FirebaseAuthProvider AuthProvider { get; set; }
        public string EmailErrorMessage
        {
            get => emailErrorMessage;
            set => SetProperty(ref emailErrorMessage, value);
        }

        public bool EmailHasError
        {
            get => emailHasError;
            set => SetProperty(ref emailHasError, value);
        }

        public AsyncCommand PageAppearingCommand { get; }
        public string PassErrorMessage
        {
            get => passErrorMessage;
            set => SetProperty(ref passErrorMessage, value);
        }

        public bool PassHasError
        {
            get => passHasError;
            set => SetProperty(ref passHasError, value);
        }

        public AsyncCommand RegisterCommand { get; }
        public string RegisterEmailEntry
        {
            get => regiserEmail;
            set
            {
                EmailHasError = false;
                SetProperty(ref regiserEmail, value);
            }
        }

        public string RegisterPasswordEntry
        {
            get => registerPass;
            set
            {
                EmailHasError = false;
                SetProperty(ref registerPass, value);
            }
        }

        public async Task OnAppearing()
        {
            ResetEntries();
        }

        private List<StatAttribute> CreateAttributes()
        {
            return new List<StatAttribute>
                {
                    new StatAttribute()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "STRENGTH",
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1,
                        ImagePath = "XamarinTodoApp.Resources.AttributeIcons.gym.png"
                    },
                    new StatAttribute()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "INTELLIGENCE",
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1,
                        ImagePath = "XamarinTodoApp.Resources.AttributeIcons.3330916.png"
                    },
                    new StatAttribute()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "SOCIAL",
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1,
                        ImagePath = "XamarinTodoApp.Resources.AttributeIcons.2228649.png"
                    },
                    new StatAttribute()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "CREATIVITY",
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1,
                        ImagePath = "XamarinTodoApp.Resources.AttributeIcons.877637.png"
                    }
                };
        }

        private async Task OnRegisterClicked()
        {
            try
            {
                EmailHasError = false;
                PassHasError = false;
                EmailErrorMessage = "Invalid email";
                PassErrorMessage = "Invalid password";

                if (string.IsNullOrEmpty(RegisterEmailEntry))
                {
                    EmailHasError = true;
                    EmailErrorMessage = "Enter an email";
                }
                else if (!new EmailAddressAttribute().IsValid(RegisterEmailEntry))
                {
                    EmailHasError = true;
                    EmailErrorMessage = "Enter a valid email";
                }

                if (RegisterPasswordEntry.Length < 6)
                {
                    PassHasError = true;
                    PassErrorMessage = "Enter at least 6 characters";
                }

                if (PassHasError || EmailHasError)
                    return;

                IsBusy = true;
                var exception = await AuthService.CreateUser(RegisterEmailEntry, RegisterPasswordEntry);
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

                UserData newUser = new UserData()
                {
                    Id = AuthService.UserInfo.LocalId,
                    Coins = 100,
                    LevelData = new LevelData()
                    {
                        CurrentExperience = 0,
                        ExperienceToNextLevel = 25,
                        Level = 1
                    },
                    StatAttributes = CreateAttributes()
                };
                //await UserDataStore.AddItemAsync(newUser);
                await UserDataStore.UpdateItemAsync(newUser.Id, newUser);
                await Shell.Current.GoToAsync("//LoginPage");
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ResetEntries()
        {
            RegisterPasswordEntry = "";
            RegisterEmailEntry = "";
            EmailHasError = false;
            PassHasError = false;
            EmailErrorMessage = "Invalid email";
            PassErrorMessage = "Invalid password";
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
    }
}