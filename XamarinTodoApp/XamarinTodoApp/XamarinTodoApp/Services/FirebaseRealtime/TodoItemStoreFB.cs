using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;
using Firebase.Database;
namespace XamarinTodoApp.Services
{
    public class TodoItemStoreFB : FirebaseDB<TodoItem>
    {
        public TodoItemStoreFB() : base("todos")
        {
            
        }
    }
}
