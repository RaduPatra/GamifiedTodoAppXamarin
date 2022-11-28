using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services.Interfaces
{
    public interface IInventoryService : IDataStore<InventoryItem>
    {
        Task AddInventoryItemAsync(InventoryItem item, string userId);
        Task UpdateInventoryItemAsync(InventoryItem item, string itemId, string userId);
    }
}
