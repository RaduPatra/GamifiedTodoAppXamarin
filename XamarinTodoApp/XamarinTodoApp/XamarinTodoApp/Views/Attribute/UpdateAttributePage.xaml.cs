using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamarinTodoApp.ViewModels.Attribute;
using Xamarin.Forms.Xaml;

namespace XamarinTodoApp.Views.Attribute
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateAttributePage : ContentPage
    {
        public UpdateAttributePage()
        {
            InitializeComponent();
            BindingContext = new AttributeDetailViewModel();
        }
    }
}