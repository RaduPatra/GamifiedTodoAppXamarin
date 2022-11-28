using Google.Cloud.Firestore;
using System;

namespace XamarinTodoApp.Models
{
    public class Item : IDataBaseItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}