using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class UserData : IDataBaseItem
    {

        //one to one with user
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public int Coins { get; set; }
        [FirestoreProperty]
        public LevelData LevelData { get; set; }
        [FirestoreProperty]
        public List<StatAttribute> StatAttributes { get; set; } = new List<StatAttribute>();
    }
}
