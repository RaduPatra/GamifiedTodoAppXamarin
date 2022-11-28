using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTodoApp.Helpers;
using XamarinTodoApp.Models;
using XamarinTodoApp.Resources;
using XamarinTodoApp.ViewModels.Attribute;
using System.Collections.ObjectModel;
using System.Linq;
using XamarinTodoApp.Services.Interfaces;

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

        public ObservableCollection<AttributeItemViewModel> Attributes { get; set; }
        public UserDataViewModel()
        {
            SetupMessages();
        }

        private void SetupMessages()
        {
            Attributes = new ObservableCollection<AttributeItemViewModel>();
            MessagingCenter.Subscribe<ViewModelBase, PayloadXP>(this, MessageChannel.UserXPChanged, async (sender, data) =>
            {
                if (data.ExpAmount > 0)
                    await AddExperience(data.ExpAmount, data.TodoAttributes);
                else if (data.ExpAmount < 0)
                    await RemoveExperience(data.ExpAmount, data.TodoAttributes);

                data.Callback(UserData);
            });


            MessagingCenter.Subscribe<ViewModelBase, PayloadCoins>(this, MessageChannel.UserCoinsChanged, async (sender, data) =>
            {
                await AddCoins(data.CoinsAmount);
                data.Callback(UserData);
            });

            MessagingCenter.Subscribe<ViewModelBase, StatAttribute>(this, MessageChannel.UserAddAttribute, async (sender, data) =>
            {
                await AddAttribute(data);
                //data.Callback(UserData);
            });

            MessagingCenter.Subscribe<ViewModelBase, StatAttribute>(this, MessageChannel.UserRemoveAttribute, async (sender, data) =>
            {
                await RemoveAttribute(data);
                //data.Callback(UserData);
            });

            MessagingCenter.Subscribe<ViewModelBase, StatAttribute>(this, MessageChannel.UpdateUserAttribute, async (sender, data) =>
            {
                await UpdateAttribute(data);
                //data.Callback(UserData);
            });


            MessagingCenter.Subscribe<ViewModelBase>(this, MessageChannel.RefreshUserData, async (sender) =>
            {
                RefreshBindings();
            });
        }
        public async Task Init()
        {
            //UserData = await UserDataStore.GetItemAsync(tempId);
            UserData = await UserDataStore.GetItemAsync(AuthService.UserInfo.LocalId);
            RefreshBindings();
        }

        private void RefreshBindings()
        {
            Level = UserData.LevelData.Level.ToString();
            Experience = UserData.LevelData.CurrentExperience.ToString();
            ExperienceToNextLevel = UserData.LevelData.ExperienceToNextLevel.ToString();
            Coins = UserData.Coins.ToString();

            UpdateAttributes();
            CalculateProgress();

        }
        private void UpdateAttributes()
        {
            try
            {
                Attributes.Clear();
                var attributeItemVMs = UserData.StatAttributes.Select(x => new AttributeItemViewModel(x)).ToList();
                foreach (var item in attributeItemVMs)
                {
                    Attributes.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        void CalculateAddLevel(IAttribute attribute, int amount)
        {
            attribute.CurrentExperience += amount;
            while (attribute.CurrentExperience > CalcNextLevel(attribute.Level))
            {
                attribute.CurrentExperience -= CalcNextLevel(attribute.Level);
                attribute.Level++;
                attribute.ExperienceToNextLevel = CalcNextLevel(attribute.Level);
            }
        }
        private async Task AddExperience(int amount, List<TodoAttribute> attributes)
        {
            var overallAmount = amount / 5;


            if (attributes.Count != 0)
            {
                var attributesAmount = amount / attributes.Count;
                foreach (var attr in attributes)
                {
                    var stat = UserData.StatAttributes.Find(st => st.Id == attr.Id);
                    CalculateAddLevel(stat, attributesAmount);
                }
            }

            CalculateAddLevel(UserData.LevelData, overallAmount);

            RefreshBindings();
            await UserDataStore.UpdateItemAsync(AuthService.UserInfo.LocalId, UserData);
        }

        void CalculateRemoveLevel(IAttribute attribute, int amount)
        {
            attribute.CurrentExperience += amount;
            while (attribute.CurrentExperience < 0)
            {
                attribute.CurrentExperience += CalcNextLevel(attribute.Level - 1);
                attribute.Level--;
                attribute.ExperienceToNextLevel = CalcNextLevel(attribute.Level);
            }
        }

        private async Task RemoveExperience(int amount, List<TodoAttribute> attributes)
        {
            var overallAmount = amount / 5;

            if (attributes.Count != 0)
            {
                var attributesAmount = amount / attributes.Count;
                foreach (var attr in attributes)
                {
                    var stat = UserData.StatAttributes.Find(st => st.Id == attr.Id);
                    CalculateRemoveLevel(stat, attributesAmount);
                }
            }

            CalculateRemoveLevel(UserData.LevelData, overallAmount);
            RefreshBindings();

            await UserDataStore.UpdateItemAsync(AuthService.UserInfo.LocalId, UserData);
        }



        private int CalcNextLevel(int lvl)
        {
            if (lvl < 5)
            {
                return 25 * lvl;
            }
            if (lvl == 5)
            {
                return 150;
            }
            return (int)Math.Round((Math.Pow(lvl, 2) * 0.25 + 10 * lvl + 139.75) / 10) * 10;
        }

        private async Task AddCoins(int amount)
        {
            UserData.Coins += amount;
            RefreshBindings();
            //await UserDataStore.UpdateItemAsync(tempId, UserData);
            await UserDataStore.UpdateItemAsync(AuthService.UserInfo.LocalId, UserData);


        }

        private async Task AddAttribute(StatAttribute attribute)
        {
            UserData.StatAttributes.Add(attribute);
            RefreshBindings();
            await UserDataStore.UpdateItemAsync(AuthService.UserInfo.LocalId, UserData);
        }


        private async Task RemoveAttribute(StatAttribute attribute)
        {
            UserData.StatAttributes.Remove(attribute);
            RefreshBindings();
            await UserDataStore.UpdateItemAsync(AuthService.UserInfo.LocalId, UserData);
        }
        private async Task UpdateAttribute(StatAttribute newStat)
        {

            var statIndex = UserData.StatAttributes.FindIndex(x => x.Id == newStat.Id);

            UserData.StatAttributes[statIndex] = newStat;
            RefreshBindings();
            await UserDataStore.UpdateItemAsync(AuthService.UserInfo.LocalId, UserData);
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

            foreach (var item in Attributes)
            {
                item.CalculateProgress();
            }
        }

        public string Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }
    }
}
