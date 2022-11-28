using Google.Cloud.Firestore;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services.Interfaces;

namespace XamarinTodoApp.ViewModels.Attribute
{
    public class AttributeItemViewModel : ObservableObject, IListItem
    {
        private bool isSelected;
        private string progress;
        public AttributeItemViewModel(StatAttribute attribute)
        {
            Item = attribute;
        }

        public StatAttribute Item { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public void CalculateProgress()
        {
            float calcProgress = (float)Item.CurrentExperience / (float)Item.ExperienceToNextLevel;
            Progress = calcProgress.ToString();
        }

        public string Progress
        {
            get
            {
                return progress;
            }
            set => SetProperty(ref progress, value);
        }
        private ImageSource image;

        public ImageSource Image
        {
            get
            {
                return ImageSource.FromResource(Item.ImagePath, typeof(AttributeItemViewModel).GetTypeInfo().Assembly);
            }
            set
            {
                SetProperty(ref image, value);
            }
        }

    }
}
