using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Services;
using XamarinTodoApp.Models;
using XamarinTodoApp.ViewModels;
using Xamarin.Forms;

namespace XamarinTodoApp.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public IDataStore<TodoItem> TodoStore => DependencyService.Get<IDataStore<TodoItem>>();
        public IDataStore<UserData> UserDataStore => DependencyService.Get<IDataStore<UserData>>();

       // public IDataStore<Item> FBStore => DependencyService.Get<IDataStore<Item>>();
    }
}
