using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Helpers
{
    public class PayloadCoins
    {
        public int CoinsAmount { get; set; }
        public Action<UserData> Callback { get; set; }
    }
}
