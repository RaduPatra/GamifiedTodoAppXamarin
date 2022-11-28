using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class TestItem2 : IDataBaseItem
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name2 { get; set; }

        [FirestoreDocumentId]
        public DocumentReference CreatedBy { get; set; }
    }
}
