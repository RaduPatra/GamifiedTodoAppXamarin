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
    public partial class RewardShopPage : ContentPage
    {

        RewardShopViewModel _viewModel;
        public RewardShopPage()
        {
            InitializeComponent();
            //testlabel.BindingContext = new NewRewardViewModel();
            BindingContext = _viewModel = new RewardShopViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }

        //private void ClickToShowPopup_Clicked(object sender, EventArgs e)
        //{
        //    popupLayout.Show();
        //}

        /*protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.OnDisappearing();
        }*/
    }
}