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
    public partial class RewardInventoryPage : ContentPage
    {
        RewardInventoryViewModel _viewModel;
        public RewardInventoryPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RewardInventoryViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }

        /*protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.OnDisappearing();
        }*/
    }
}