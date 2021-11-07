using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.ViewModels.Shop
{
    public class RewardItemViewModel : ObservableObject
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
    }
}



/*void InitImages()
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            var folderName = "XamarinTodoApp.Resources.RewardIcons";
            var fileNames = assem.GetManifestResourceNames().Where(r => r.StartsWith(folderName) && r.EndsWith(".png"));

            var first = fileNames.ToList()[0];
            SelectedImage = ImageSource.FromResource(first, typeof(NewRewardPage).GetTypeInfo().Assembly);


            foreach (var file in fileNames)
            {
                var img = ImageSource.FromResource(file, typeof(NewRewardPage).GetTypeInfo().Assembly);
                try
                {
                    Images.Add(img);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }*/
