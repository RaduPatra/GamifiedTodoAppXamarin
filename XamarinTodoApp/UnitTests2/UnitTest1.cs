using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.Services.Firestore;
using Firebase.Database;
using Xunit;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using XamarinTodoApp;
using Firebase.Auth;
using System.Linq;
using Google.Cloud.Firestore.Converters;
using Google.Cloud.Firestore.V1;

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
            var database = new FirestoreUser();

            UserData currentUserData = await database.GetItemAsync("3hohYTtPOjYEZNE7WIIL8gnDXSa2");

            UserData newUserData = new()
            {
                Id = currentUserData.Id,
                Coins = currentUserData.Coins,
                LevelData = currentUserData.LevelData,
                StatAttributes = new List<StatAttribute>()


            };
            await database.UpdateItemAsync("3hohYTtPOjYEZNE7WIIL8gnDXSa2", newUserData);
        }




        [Fact]
        public async Task FirebaseSignupTest()
        {
            string WebApiKey = "AIzaSyDFVKP9U2pNDUntJgCZg23q2E0tbjr9ef0";

            try
            {
                string email = "test123@gmail.com";
                string password = "abcdefg123";
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                string token = auth.FirebaseToken;
                //await App.Current.MainPage.DisplayAlert("Alert", token, "Ok");
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }

        [Fact]

        public async Task FirebaseLoginTest()
        {
            string WebApiKey = "AIzaSyDFVKP9U2pNDUntJgCZg23q2E0tbjr9ef0";

            try
            {
                string email = "radupatra@gmail.com";
                string password = "123456";
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                var content = await auth.GetFreshAuthAsync();
                //await App.Current.MainPage.DisplayAlert("Alert", token, "Ok");
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            }
        }


        [Fact]

        public async Task FirebaseRefTest()
        {
            string path = "D:\\Radu\\Desktop\\FirebaseKey\\xamarintodoapp-firebase-adminsdk-8u456-629604879b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            var database = new TestFire1();

            var item = new TestItem1()
            {
                Name = "test"
            };
            await database.AddItemAsync(item);
            var x = item;

        }


        [Fact]
        public async Task AddTodoTest()
        {
            string path = "D:\\Radu\\Desktop\\FirebaseKey\\xamarintodoapp-firebase-adminsdk-8u456-629604879b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            var database = new FirestoreTodo();
            var fbkey = FirebaseKeyGenerator.Next();
            DateTime date = DateTime.Now.ToUniversalTime();

            var stats = new List<StatAttribute>();

            /*stats.Add(new Todoattr() { Name = "statTest", ImagePath = "imageTest" });

            TodoItem newItem = new TodoItem()
            {
                Id = "bruh",
                Text = "test",
                IsDone = false,
                Reward = 1,
                ExpReward = 1,
                CreationDate = Timestamp.FromDateTime(date),
                StatAttributes = stats,
                DueDate = null

            };

            await database.AddItemAsync(newItem);*/


        }


        [Fact]
        public async Task TestRefSnap()
        {
            string path = "D:\\Radu\\Desktop\\FirebaseKey\\xamarintodoapp-firebase-adminsdk-8u456-629604879b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            string projectId = "xamarintodoapp";
            database = new FirestoreTodo();


            var items = await database.GetItemsAsync();


            FirestoreDb db = FirestoreDb.Create(projectId);

            CollectionReference coll = db.Collection("testfire1");

            var userDocRef = await db.Collection("userdata").Document("userIdTemp").GetSnapshotAsync();
            /*todoItem.CreatedBy = userDocRef.Reference;
            todoItem.CreatedBy.
            await AddItemAsync(todoItem);*/

            var item2 = await db.Collection("testfire2").Document("iD6r1RsvpqfRvzSoRnV5").GetSnapshotAsync();
            //item2.Refe

            var item = new TestItem1()
            {
                Name = "test567",
                //DocReference = item2.Reference
            };

            //data1.Add("Text", "123");
            DocumentReference docRef = await coll.AddAsync(item);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            TestItem1 testItem = snapshot.ConvertTo<TestItem1>();
            var x = testItem;
            //await doc.SetAsync(data1);

        }


        [Fact]
        public async Task GetUserTest()
        {
            var tempId = "userIdTemp";
            string path = "D:\\Radu\\Desktop\\FirebaseKey\\xamarintodoapp-firebase-adminsdk-8u456-629604879b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            var database = new FirestoreUser();

            UserData currentUserData = await database.GetItemAsync("3hohYTtPOjYEZNE7WIIL8gnDXSa2");

            //await database.UpdateItemAsync(tempId, newUserData);
        }


        [Fact]
        public async Task DateTest()
        {
            DateTime DueDate = DateTime.Now.Date;
            DateTime creationDate = DateTime.Now.ToUniversalTime();
            DateTime dueDate = DueDate.ToUniversalTime();
            var d1 = Timestamp.FromDateTime(dueDate);
            var d2 = Timestamp.FromDateTime(creationDate);
            var d3 = d1.ToDateTime().Date;
            var d4 = d3.ToString("dd/MM/yyyy");

        }

        [Fact]
        public async Task DateTest2()
        {
            DateTime? DueDate = null;
            DateTime? NowDate = DateTime.Now;

            DateTime? dueDate = null;
            if (DueDate != null)
                dueDate = ((DateTime)DueDate).ToUniversalTime();
            DateTime nowDate = ((DateTime)NowDate).ToUniversalTime();
        }


        int CurrentExperience = 30;
        int Level = 5;
        [Fact]
        public void AddExperience()
        {

            CurrentExperience += 700;
            while (CurrentExperience >= CalcNextLevel(Level))
            {
                LevelUp();
            }

            //await UserDataStore.UpdateItemAsync(tempId, UserData);
        }


        void LevelUp()
        {
            CurrentExperience -= CalcNextLevel(Level);
            Level++;
        }
        private int CalcNextLevel(int lvl)
        {
            if (lvl < 5)
            {
                return 25 * lvl;
            }
            if (lvl == 5)
            {
                return 150;
            }
            return (int)Math.Round((Math.Pow(lvl, 2) * 0.25 + 10 * lvl + 139.75) / 10) * 10;
        }

        [Fact]
        private async Task RemoveExperience()
        {
            CurrentExperience -= 80;
            while (CurrentExperience < 0)
            {
                CurrentExperience += CalcNextLevel(Level);
                Level--;
            }

        }


    }



}

