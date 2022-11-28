using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Services.Interfaces;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class LevelData : IAttribute
    {

        [FirestoreProperty]
        public int Level { get; set; }
        [FirestoreProperty]
        public int CurrentExperience { get; set; }
        [FirestoreProperty]
        public int ExperienceToNextLevel { get; set; }
    }
}
