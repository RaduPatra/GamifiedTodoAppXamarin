using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTodoApp.ViewModels.Attribute;

namespace XamarinTodoApp.Views.Attribute
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttributePage : ContentPage
    {
        public AttributePage()
        {
            InitializeComponent();
            BindingContext = new AttributeViewModel();
        }
    }
}