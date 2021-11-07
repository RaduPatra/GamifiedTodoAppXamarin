using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services
{

    public interface IApplicationCache
    {
        UserData UserData { get; set; }
    }
    public class ApplicationCache : IApplicationCache
    {
        public UserData UserData { get; set; }
    }
}
