using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using Firebase.Database;
using Xunit;
namespace UnitTests2
{
    public class UnitTest1
    {
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
    }
}
