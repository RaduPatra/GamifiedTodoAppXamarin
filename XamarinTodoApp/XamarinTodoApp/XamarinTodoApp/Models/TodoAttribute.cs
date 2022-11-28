using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Firestore;
using Xamarin.Forms;


namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class TodoAttribute
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string ImagePath { get; set; }
    }
}
