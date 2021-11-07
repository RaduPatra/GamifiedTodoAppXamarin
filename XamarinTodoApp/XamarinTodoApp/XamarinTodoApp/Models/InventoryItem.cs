using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class InventoryItem : IDataBaseItem
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public RewardItem RewardItem { get; set; }

        [FirestoreProperty]
        public int Quantity { get; set; }
    }
}
