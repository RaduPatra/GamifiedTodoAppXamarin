using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Helpers
{
    internal class PayloadAttribute
    {
        public StatAttribute Attribute { get; set; }
        public Action<UserData> Callback { get; set; }
    }
}

