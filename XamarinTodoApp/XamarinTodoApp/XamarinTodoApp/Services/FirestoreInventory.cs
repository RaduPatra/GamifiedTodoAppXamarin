using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;
namespace XamarinTodoApp.Services
{
    public class FirestoreInventory : FirestoreDatabase<InventoryItem>
    {

        public FirestoreInventory() : base("inventory")
        {

        }
    }
}
