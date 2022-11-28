using Google.Cloud.Firestore;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.ViewModels.Todo
{
    public class TodoAttributeViewModel : ObservableObject
    {
        public TodoAttributeViewModel(TodoAttribute attribute)
        {
            Item = attribute;
        }

        public TodoAttribute Item { get; set; }

        public string NameTest { get => Item.Name; }




        private ImageSource image;

        public ImageSource Image
        {
            get
            {
                return ImageSource.FromResource(Item.ImagePath, typeof(TodoAttributeViewModel).GetTypeInfo().Assembly);
                //return ImageSource.FromResource("XamarinTodoApp.Resources.AttributeIcons.exercise.png", typeof(TodoAttributeViewModel).GetTypeInfo().Assembly);
            }
            set
            {
                SetProperty(ref image, value);
            }
        }

    }
}
