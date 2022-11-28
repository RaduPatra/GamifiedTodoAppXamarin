using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

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

        public string Description { get; set; }
        [FirestoreProperty]
        public bool IsDone { get; set; }
        [FirestoreProperty]
        public int Reward { get; set; }

        [FirestoreProperty]
        public int ExpReward { get; set; }

        [FirestoreProperty]
        public DocumentReference CreatedBy { get; set; }


        [FirestoreProperty]
        public Timestamp CreationDate { get; set; }

        //todo - make converter for timestamp->datetime in get, and datetime -> timestamp in set
        [FirestoreProperty]
        public Timestamp? DueDate { get; set; }

        [FirestoreProperty]
        public Timestamp? ReminderDate { get; set; }

        [FirestoreProperty]
        public int RepeatFrequency { get; set; }

        [FirestoreProperty]
        public int Difficulty { get; set; }

        [FirestoreProperty]
        public List<TodoAttribute> StatAttributes { get; set; }

        [FirestoreProperty]
        public bool HasCustomCoinReward { get; set; } = false;

        [FirestoreProperty]
        public bool HasCustomExpReward { get; set; } = false;

        [FirestoreProperty]
        public int NotificationId { get; set; }

    }
}
