using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTodoApp.Views.Attribute
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAttributeTemplate : ContentView
    {
        public NewAttributeTemplate()
        {
            InitializeComponent();
        }

        protected virtual void OnAppearing()
        {
        }
    }
}