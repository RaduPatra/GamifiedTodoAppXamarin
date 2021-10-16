using System.ComponentModel;
using Xamarin.Forms;
using XamarinTodoApp.ViewModels;

namespace XamarinTodoApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}