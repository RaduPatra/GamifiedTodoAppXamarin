using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using XamarinTodoApp.Models;
namespace XamarinTodoApp.Services
{
    public class FirestoreReward : FirestoreDatabase<RewardItem>
    {

        public FirestoreReward() : base("rewards")
        {

        }
    }
}
