using System;
using System.Collections.Generic;
using System.Text;
using Google.Cloud.Firestore;
using Xamarin.Forms;
using XamarinTodoApp.Services.Interfaces;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class StatAttribute : IDataBaseItem, IAttribute
    {

        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string ImagePath { get; set; }

        [FirestoreProperty]
        public int Level { get; set; }
        [FirestoreProperty]
        public int CurrentExperience { get; set; }
        [FirestoreProperty]
        public int ExperienceToNextLevel { get; set; }


    }
}
