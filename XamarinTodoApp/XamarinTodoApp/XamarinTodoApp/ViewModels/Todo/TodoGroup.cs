using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace XamarinTodoApp.ViewModels.Todo
{
    public class TodoGroup : ObservableCollection<TodoItemViewModel>
    {
        public string GroupName { get; set; }
        public TodoGroup(string groupName, ObservableCollection<TodoItemViewModel> todos) : base(todos)
        {
            GroupName = groupName;
        }

    }
}
