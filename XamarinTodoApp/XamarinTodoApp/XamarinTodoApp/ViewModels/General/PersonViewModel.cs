using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

using XamarinTodoApp.Models;
using XamarinTodoApp.Services;

namespace XamarinTodoApp.ViewModels
{
    class PersonViewModel
    {
        public List<Person> Data { get; set; }

        public PersonViewModel()
        {
            Data = new List<Person>()
            {
                new Person { Name = "David", Height = 180 },
                new Person { Name = "Michael", Height = 170 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Joel", Height = 182 }
            };
        }
    }
}
