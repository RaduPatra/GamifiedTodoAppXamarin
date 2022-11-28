using Google.Cloud.Firestore;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using XamarinTodoApp.Models;
using XamarinTodoApp.ViewModels.Attribute;
using System.Linq;
using XamarinTodoApp.Services.Interfaces;

namespace XamarinTodoApp.ViewModels.Todo
{
    public delegate void CustomAction(TodoItemViewModel itemVM, bool isChecked, bool autoUpdate = false);
    public class TodoItemViewModel : ObservableObject, IListItem
    {
        private readonly Action<TodoItemViewModel, bool> itemAction;
        //delegate void CustomAction(TodoItemViewModel itemVM, bool isChecked, bool autoUpdate=false);
        private readonly CustomAction customAction;
        private bool isChecked;
        public bool isSelected;
        Timestamp? itemDueDate;
        public ObservableCollection<TodoAttributeViewModel> TodoAttributes { get; set; }


        public TodoItemViewModel(TodoItem item, CustomAction customAction)
        {
            Item = item;
            itemDueDate = Item.DueDate;
            isChecked = item.IsDone;
            //this.itemAction = itemAction;
            this.customAction = customAction;

            var attrVMs = item.StatAttributes.Select(x => new TodoAttributeViewModel(x));
            TodoAttributes = new ObservableCollection<TodoAttributeViewModel>();
            foreach (var attr in attrVMs)
            {
                TodoAttributes.Add(attr);
            }


        }

        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked,
                value, string.Empty,
                () =>
                {
                    //itemAction.Invoke(Item, value);
                    customAction.Invoke(this, value);
                });
        }

        public bool IsCheckedAuto
        {
            get => isChecked;
            set => SetProperty(ref isChecked,
                value, string.Empty,
                () =>
                {
                    //itemAction.Invoke(Item, value);
                    customAction.Invoke(this, value, true);
                });
        }

        public bool IsCheckedVisual
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public Timestamp? ItemDueDate
        {
            get => itemDueDate;
            set => SetProperty(ref itemDueDate, value, string.Empty,
                () =>
                {
                    //itemAction.Invoke(Item, value);
                    Console.WriteLine("prop changed");

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