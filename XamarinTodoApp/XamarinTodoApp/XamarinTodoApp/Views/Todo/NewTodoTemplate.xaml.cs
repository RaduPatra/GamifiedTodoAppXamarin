using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTodoApp.Views.Todo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTodoTemplate : ContentView
    {
        public NewTodoTemplate()
        {
            InitializeComponent();
        }
    }
}