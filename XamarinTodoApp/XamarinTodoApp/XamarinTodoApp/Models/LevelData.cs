using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTodoApp.Models
{
    [FirestoreData]
    public class LevelData
    {

        [FirestoreProperty]
        public int Level { get; set; }
        [FirestoreProperty]
        public int CurrentExperience { get; set; }
        [FirestoreProperty]
        public int ExperienceToNextLevel { get; set; }
    }
}
