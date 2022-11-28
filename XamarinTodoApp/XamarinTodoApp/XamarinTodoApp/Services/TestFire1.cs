using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services
{
    public class TestFire1 : FirestoreDatabase<TestItem1>
    {
        public TestFire1() : base("testfire1")
        {

        }
    }
}
