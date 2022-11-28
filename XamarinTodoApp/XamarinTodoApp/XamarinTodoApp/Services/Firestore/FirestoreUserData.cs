using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services
{
    public class FirestoreUserData : FirestoreDatabase<UserData>
    {
        public FirestoreUserData() : base("userdata")
        {

        }
    }
}
