using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTodoApp.ViewModels.Shop;

namespace XamarinTodoApp.Views.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRewardPage : ContentPage
    {
        NewRewardViewModel viewModel;
        public NewRewardPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewRewardViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}