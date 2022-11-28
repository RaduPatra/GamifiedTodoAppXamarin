using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using XamarinTodoApp.Models;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace XamarinTodoApp.Services
{
    public class FirestoreTodo : FirestoreDatabase<TodoItem>, ITodoService
    {

        public FirestoreTodo() : base("todos")
        {

        }

        public async Task AddTodoAsync(TodoItem item, string userId)
        {
            var userDocRef = db.Collection("users").Document(userId);
            item.CreatedBy = userDocRef;
            await AddItemAsync(item);
        }

    }
}
