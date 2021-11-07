using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Services;
using XamarinTodoApp.Models;
using XamarinTodoApp.ViewModels;
using Xamarin.Forms;
using XamarinTodoApp.ViewModels.General;

namespace XamarinTodoApp.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        protected readonly string tempId = "userIdTemp";
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public IDataStore<TodoItem> TodoStore => DependencyService.Get<IDataStore<TodoItem>>();
        public IDataStore<UserData> UserDataStore => DependencyService.Get<IDataStore<UserData>>();
        public IDataStore<RewardItem> RewardStore => DependencyService.Get<IDataStore<RewardItem>>();
        public IDataStore<InventoryItem> InventoryStore => DependencyService.Get<IDataStore<InventoryItem>>();

        //public IDataStore<UserDataViewModel> InventoryStore => DependencyService.Get<IDataStore<InventoryItem>>();

        //public UserDataViewModel UserDataViewModel => DependencyService.Get<UserDataViewModel>();



        public IApplicationCache ApplicationCache => DependencyService.Get<IApplicationCache>();



        //public IDataStore<TodoItem> TodoFirestore => DependencyService.Get<IDataStore<TodoItem>>();



        // public IDataStore<Item> FBStore => DependencyService.Get<IDataStore<Item>>();
    }
}
