using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTodoApp.Models
{
    public class UserData : IDataBaseItem
    {

        //one to one with user
        public string Id { get; set; }

        public int Coins { get; set; }
    }
}
