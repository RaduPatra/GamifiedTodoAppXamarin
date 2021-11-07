using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using Firebase.Database;
using Xunit;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using XamarinTodoApp;

namespace UnitTests2
{
    public class UnitTest1
    {
        FirestoreTodo database;
        public UnitTest1()
        {
            string path = "D:\\Radu\\Desktop\\FirebaseKey\\xamarintodoapp-firebase-adminsdk-8u456-629604879b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            database = new FirestoreTodo();
        }


        [Fact]
        public async Task Test2()
        {
            var fbId = FirebaseKeyGenerator.Next();
            TodoItem newItem = new TodoItem()
            {
                Id = fbId,
                Text = "new item test " + fbId,
                IsDone = false,
                Reward = 9999
            };
            var dataStore = new TodoItemStoreFB();
            var res = await dataStore.AddItemAsync(newItem);

        }

        [Fact]
        public async Task Test3()
        {
            var fbId = FirebaseKeyGenerator.Next();
            UserData newItem = new UserData()
            {
                Id = fbId,
                Coins = 0
            };
            var dataStore = new UserDataFB();
            var res = await dataStore.AddItemAsync(newItem);

        }

        [Fact]
        public async Task Test4()
        {
            string path = "D:\\Radu\\Desktop\\FirebaseKey\\xamarintodoapp-firebase-adminsdk-8u456-629604879b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            string projectId = "xamarintodoapp";
            FirestoreDb db = FirestoreDb.Create(projectId);

            CollectionReference coll = db.Collection("items");
            //DocumentReference coll = db.Collection("items").Document("item2");




            Dictionary<string, object> data1 = new Dictionary<string, object>()
            {
                    {"Text", "123"},
            };
            //data1.Add("Text", "123");
            await coll.AddAsync(data1);
            //await doc.SetAsync(data1);

        }
        [Fact]
        public void Test5()
        {
            var content = App.GetFromResources("Resources.credentials.json");
            Assert.NotNull(content);
        }

        [Fact]
        public async Task DeleteAllTodos()
        {
            string path = "D:\\Radu\\Desktop\\FirebaseKey\\xamarintodoapp-firebase-adminsdk-8u456-629604879b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            var database = new FirestoreTodo();
            IEnumerable<TodoItem> items = await database.GetItemsAsync();
            foreach (var item in items)
            {
                await database.DeleteItemAsync(item.Id);
            }
        }


        [Fact]
        public void Test6()
        {
            var content = App.GetFilenamesFromFolder();
            Assert.NotNull(content);
        }

        [Fact]

        public async Task UserDataMigration()
        {
            var tempId = "userIdTemp";
            string path = "D:\\Radu\\Desktop\\FirebaseKey\\xamarintodoapp-firebase-adminsdk-8u456-629604879b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            var database = new FirestoreUserData();

            UserData currentUserData = await database.GetItemAsync(tempId);

            UserData newUserData = new()
            {
                Id = "userIdTemp",
                Coins = currentUserData.Coins,
                LevelData = new LevelData()
                {
                    CurrentExperience = 0,
                    ExperienceToNextLevel = 100,
                    Level = 1
                }


            };
            await database.UpdateItemAsync(tempId, newUserData);
        }
    }



}

