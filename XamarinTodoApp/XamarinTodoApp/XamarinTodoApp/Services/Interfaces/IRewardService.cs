using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services.Interfaces
{
    public interface IRewardService : IDataStore<RewardItem>
    {
        Task AddRewardAsync(RewardItem item, string userId);

    }
}
