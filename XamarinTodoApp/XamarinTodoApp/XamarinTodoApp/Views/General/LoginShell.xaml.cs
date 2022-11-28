using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using Xamarin.Essentials;
using XamarinTodoApp.ViewModels;
using XamarinTodoApp.Resources;
using XamarinTodoApp.Views;
using XamarinTodoApp.Views.Todo;
using XamarinTodoApp.Views.Shop;
using XamarinTodoApp.Services;
using System.Collections.ObjectModel;
using XamarinTodoApp.Views.Attribute;

namespace XamarinTodoApp.Views.General
{
    public partial class LoginShell : Xamarin.Forms.Shell
    {
        public LoginShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            //Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }
    }
}