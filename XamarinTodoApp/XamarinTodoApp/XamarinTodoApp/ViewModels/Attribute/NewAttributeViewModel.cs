using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;
using XamarinTodoApp.Resources;


using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinTodoApp.Services;
using XamarinTodoApp.ViewModels.Todo;
//using XamarinTodoApp.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using XamarinTodoApp.Views.Todo;
using Google.Cloud.Firestore;
using System.Linq;
using System.Reflection;
using Syncfusion.XForms.PopupLayout;
using XamarinTodoApp.Views.Attribute;
using XamarinTodoApp.Helpers;

namespace XamarinTodoApp.ViewModels.Attribute
{
    public class NewAttributeViewModel : NewAttributeBaseViewModel
    {

        public NewAttributeViewModel()
        {

        }

        public override async void OnSave()
        {
            base.OnSave();
            var stat = new StatAttribute()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Text,
                CurrentExperience = 0,
                Level = 1,
                ExperienceToNextLevel = 25,
                ImagePath = SelectedPath
                //todo img path
            };
            MessagingCenter.Send<ViewModelBase, StatAttribute>(this, MessageChannel.UserAddAttribute, stat);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");

        }

    }
}
