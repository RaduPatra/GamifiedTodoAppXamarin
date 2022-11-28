using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services.Interfaces;
namespace XamarinTodoApp.Services
{
    public class FirestoreInventory : FirestoreDatabase<InventoryItem> , IInventoryService
    {

        public FirestoreInventory() : base("inventory")
        {

        }

        public async Task AddInventoryItemAsync(InventoryItem item, string userId)
        {
            var userDocRef = db.Collection("users").Document(userId);//
            item.CreatedBy = userDocRef;
            await AddItemAsync(item);
        }

        public async Task UpdateInventoryItemAsync(InventoryItem item, string itemId, string userId)
        {
            var userDocRef = db.Collection("users").Document(userId);
            item.CreatedBy = userDocRef;
            await UpdateItemAsync(itemId, item);
        }
    }
}
