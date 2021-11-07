using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.ViewModels.Todo
{
    public class TodoItemViewModel : ObservableObject
    {
        private readonly Action<TodoItem, bool> itemAction;
        private bool isChecked;
        public TodoItemViewModel(TodoItem item, Action<TodoItem, bool> itemAction)
        {
            Item = item;
            isChecked = item.IsDone;
            this.itemAction = itemAction;

        }

        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked,
                value, string.Empty,
                () =>
                {
                    itemAction.Invoke(Item, value);
                });
        }

        public TodoItem Item { get; set; }
    }
}



/*public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }*/