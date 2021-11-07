using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinTodoApp.Models;
using XamarinTodoApp.Services;
using XamarinTodoApp.ViewModels.Todo;
//using XamarinTodoApp.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinTodoApp.Views.Todo;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Syncfusion.XForms.PopupLayout;
using XamarinTodoApp.Views.Shop;
using XamarinTodoApp.Helpers;

namespace XamarinTodoApp.ViewModels.Shop
{
    public class NewRewardViewModel : ViewModelBase
    {
        private string text;



        private string price;
        private SfPopupLayout currentPopup;
        private ImageSource selectedimage;
        private string selectedPath;
        public ObservableCollection<ImageInfo> Images { get; set; }

        public Command<ImageInfo> ImageSelectedCommand { get; set; }

        public Command<SfPopupLayout> OpenPopupCommand { get; }




        public NewRewardViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Images = new ObservableCollection<ImageInfo>();

            ImageSelectedCommand = new Command<ImageInfo>(OnImageSelected);
            OpenPopupCommand = new Command<SfPopupLayout>(OnOpenPopup);


        }

        void OnImageSelected(ImageInfo imageSource)
        {
            SelectedImage = imageSource.ImgSource;
            selectedPath = imageSource.ImagePath;
            currentPopup.Dismiss();
        }

        void OnOpenPopup(SfPopupLayout popupLayout)
        {
            currentPopup = popupLayout;
            popupLayout.Show();
        }

        void InitImages()
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            var folderName = "XamarinTodoApp.Resources.RewardIcons";
            var fileNames = assem.GetManifestResourceNames().Where(r => r.StartsWith(folderName) && r.EndsWith(".png"));

            var defaultIcon = $"{folderName}.diamond.png";
            SelectedImage = ImageSource.FromResource(defaultIcon, typeof(NewRewardPage).GetTypeInfo().Assembly);


            foreach (var file in fileNames)
            {
                var imgSource = ImageSource.FromResource(file, typeof(NewRewardPage).GetTypeInfo().Assembly);
                ImageInfo newImageInfo = new ImageInfo()
                {
                    ImagePath = file,
                    ImgSource = imgSource
                };
                try
                {
                    Images.Add(newImageInfo);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(price);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        public ImageSource SelectedImage
        {
            get => selectedimage;
            set => SetProperty(ref selectedimage, value);
        }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }


        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public async Task OnAppearing()
        {
            InitImages();
            //SelectedItem = null;
        }

        private async void OnSave()
        {
            DateTime date = DateTime.Now.ToUniversalTime();
            RewardItem newItem = new RewardItem()
            {
                Id = "",
                Text = Text,
                Price = int.Parse(Price),
                CreationDate = Timestamp.FromDateTime(date),
                ImagePath = selectedPath
            };

            await RewardStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

    }

    
}
