using Google.Cloud.Firestore;
using System;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class TodoItem : IDataBaseItem
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Text { get; set; }
        [FirestoreProperty]
        public bool IsDone { get; set; }
        [FirestoreProperty]
        public int Reward { get; set; }

        [FirestoreProperty]
        public int ExpReward { get; set; }

        [FirestoreProperty]
        public Timestamp CreationDate { get; set; }

    }
}
