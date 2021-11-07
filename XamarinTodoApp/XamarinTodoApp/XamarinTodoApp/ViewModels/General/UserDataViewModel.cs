using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTodoApp.Helpers;
using XamarinTodoApp.Models;
using XamarinTodoApp.Resources;

namespace XamarinTodoApp.ViewModels.General
{
    public class UserDataViewModel : ViewModelBase
    {
        private string coins;
        private string level;
        private string experience;
        private string experienceToNextLevel;
        private string progress;
        public UserData UserData { get; set; }

        public UserDataViewModel()
        {
            SetupMessages();
        }

        private void SetupMessages()
        {
            MessagingCenter.Subscribe<ViewModelBase, PayloadXP>(this, MessageChannel.UserXPChanged, async (sender, data) =>
            {
                if (data.ExpAmount > 0)
                    await AddExperience(data.ExpAmount);
                else
                    await RemoveExperience(data.ExpAmount);

                data.Callback(UserData);
            });


            MessagingCenter.Subscribe<ViewModelBase, PayloadCoins>(this, MessageChannel.UserCoinsChanged, async (sender, data) =>
            {
                await AddCoins(data.CoinsAmount);
                data.Callback(UserData);
            });
        }
        public async Task Init()
        {
            UserData = await UserDataStore.GetItemAsync(tempId);
            RefreshBindings();
        }

        private void RefreshBindings()
        {
            Level = UserData.LevelData.Level.ToString();
            Experience = UserData.LevelData.CurrentExperience.ToString();
            ExperienceToNextLevel = UserData.LevelData.ExperienceToNextLevel.ToString();
            Coins = UserData.Coins.ToString();
            CalculateProgress();
        }

        //todo - add/remove coins

        private async Task AddExperience(int amount)
        {
            UserData.LevelData.CurrentExperience += amount;

            if (UserData.LevelData.CurrentExperience >= UserData.LevelData.ExperienceToNextLevel)
            {
                UserData.LevelData.Level++;
                UserData.LevelData.CurrentExperience -= UserData.LevelData.ExperienceToNextLevel;
                //exptonextlevel = calcxptonext()
            }

            RefreshBindings();

            await UserDataStore.UpdateItemAsync(tempId, UserData);
        }

        private async Task RemoveExperience(int amount)
        {
            UserData.LevelData.CurrentExperience += amount;

            if (UserData.LevelData.CurrentExperience < 0 && UserData.LevelData.Level > 1)
            {
                UserData.LevelData.Level--;
                UserData.LevelData.CurrentExperience = UserData.LevelData.ExperienceToNextLevel - UserData.LevelData.CurrentExperience;
                //exptonextlevel = calcxptonext()
            }

            RefreshBindings();

            await UserDataStore.UpdateItemAsync(tempId, UserData);
        }

        private async Task AddCoins(int amount)
        {
            UserData.Coins += amount;
            RefreshBindings();
            await UserDataStore.UpdateItemAsync(tempId, UserData);

        }


        public string Coins
        {
            get => coins;
            set => SetProperty(ref coins, value);
        }

        public string Level
        {
            get => level;
            set => SetProperty(ref level, value);
        }

        public string Experience
        {
            get => experience;
            set => SetProperty(ref experience, value);
        }

        public string ExperienceToNextLevel
        {
            get => experienceToNextLevel;
            set => SetProperty(ref experienceToNextLevel, value);
        }

        private void CalculateProgress()
        {
            float calcProgress = float.Parse(experience) / float.Parse(experienceToNextLevel);
            Progress = calcProgress.ToString();
        }

        public string Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }
    }
}
