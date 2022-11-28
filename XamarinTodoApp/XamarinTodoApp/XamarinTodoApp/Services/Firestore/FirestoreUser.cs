using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services.Firestore
{
    public class FirestoreUser : FirestoreDatabase<UserData>
    {
        public FirestoreUser() : base("users")
        {

        }
    }
}
