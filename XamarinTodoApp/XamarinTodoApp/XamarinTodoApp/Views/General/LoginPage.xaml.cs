using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTodoApp.ViewModels;

namespace XamarinTodoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }
    }
}