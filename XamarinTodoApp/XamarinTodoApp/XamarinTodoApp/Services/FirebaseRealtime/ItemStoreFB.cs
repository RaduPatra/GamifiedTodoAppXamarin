using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services
{
    public class ItemStoreFB : FirebaseDB<Item>
    {
        public ItemStoreFB() : base("items")
        {
        }

    }
}
