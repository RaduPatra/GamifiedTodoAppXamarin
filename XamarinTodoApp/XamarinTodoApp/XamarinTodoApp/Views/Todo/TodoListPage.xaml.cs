using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTodoApp.Models;
using XamarinTodoApp.ViewModels;
using XamarinTodoApp.Views;


namespace XamarinTodoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoListPage : ContentPage
    {
        TodoListViewModel _viewModel;

        public TodoListPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new TodoListViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.OnDisappearing();
        }

        /*private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var check = (CheckBox)sender;
            var item = (TodoItem)check.BindingContext;
            _viewModel.ExecuteCheckedTest(item);
            //Task.Run(async () => await _viewModel.ExecuteChecked(item));
        }*/
    }
}