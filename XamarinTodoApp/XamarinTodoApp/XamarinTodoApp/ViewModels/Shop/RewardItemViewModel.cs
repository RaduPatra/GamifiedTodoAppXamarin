using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services.Interfaces;

namespace XamarinTodoApp.ViewModels.Shop
{
    public class RewardItemViewModel : ObservableObject, IListItem
    {
        private ImageSource image;

        public RewardItemViewModel(RewardItem item)
        {
            Item = item;
        }

        public ImageSource Image
        {
            get
            {
                return ImageSource.FromResource(Item.ImagePath, typeof(RewardItemViewModel).GetTypeInfo().Assembly);
            }
            set
            {
                SetProperty(ref image, value);
            }
        }

        public RewardItem Item { get;  }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }
    }
}

