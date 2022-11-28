using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Firestore;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class TestItem1 : IDataBaseItem
    {
        [FirestoreProperty]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        //[FirestoreDocumentId]
        [FirestoreProperty]
        public DocumentReference CreatedBy { get; set; }
    }
}
