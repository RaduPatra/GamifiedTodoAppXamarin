using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;
namespace XamarinTodoApp.Services
{
    public class TestFire2 : FirestoreDatabase<TestItem2>
    {

        public TestFire2() : base("testfire2")
        {

        }
    }
}
