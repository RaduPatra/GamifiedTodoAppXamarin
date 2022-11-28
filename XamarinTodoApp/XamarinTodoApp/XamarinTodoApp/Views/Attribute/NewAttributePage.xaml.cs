using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamarinTodoApp.Models;
using XamarinTodoApp.ViewModels.Attribute;
using XamarinTodoApp.Views;


namespace XamarinTodoApp.Views.Attribute
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAttributePage : ContentPage
    {
        NewAttributeViewModel _viewModel;
        public NewAttributePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewAttributeViewModel();
        }
    }
}