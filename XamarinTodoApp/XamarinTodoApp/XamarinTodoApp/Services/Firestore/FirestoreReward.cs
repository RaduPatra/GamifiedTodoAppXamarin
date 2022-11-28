using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services.Interfaces;
using System.Threading.Tasks;
namespace XamarinTodoApp.Services
{
    public class FirestoreReward : FirestoreDatabase<RewardItem>, IRewardService
    {

        public FirestoreReward() : base("rewards")
        {

        }

        public async Task AddRewardAsync(RewardItem item, string userId)
        {
            var userDocRef = db.Collection("users").Document(userId);
            item.CreatedBy = userDocRef;
            await AddItemAsync(item);
        }
    }
}
