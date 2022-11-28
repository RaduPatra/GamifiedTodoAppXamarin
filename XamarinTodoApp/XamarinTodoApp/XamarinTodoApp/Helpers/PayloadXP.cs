using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Helpers
{
    public class PayloadXP
    {
        public int ExpAmount { get; set; }
        public List<TodoAttribute> TodoAttributes { get; set; }
        public Action<UserData> Callback { get; set; }
    }
}
