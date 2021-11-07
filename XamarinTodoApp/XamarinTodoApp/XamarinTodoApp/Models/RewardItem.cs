using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Firestore;
using Xamarin.Forms;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class RewardItem : IDataBaseItem
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Text { get; set; }

        [FirestoreProperty]
        public int Price { get; set; }

        [FirestoreProperty]
        public string ImagePath { get; set; }

        [FirestoreProperty]
        public Timestamp CreationDate { get; set; }
    }
}
