using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services
{
    public class FirestoreTodo : FirestoreDatabase<TodoItem>
    {
        public FirestoreTodo() : base("todos")
        {

        }
    }
}
