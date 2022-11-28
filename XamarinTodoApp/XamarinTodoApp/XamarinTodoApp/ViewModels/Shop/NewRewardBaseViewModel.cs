using System;
using System.Collections.Generic;
using System.Text;
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
    public class NewRewardBaseViewModel : ViewModelBase
    {
        private string text;
        private string description;
        public string SelectedPath { get; set; }
        private string price;
        private SfPopupLayout currentPopup;
        private ImageSource selectedimage;
        private string selectedPath;
        public ObservableCollection<ImageInfo> Images { get; set; }

        public AsyncCommand PageAppearingCommand { get; }



        public Command<ImageInfo> ImageSelectedCommand { get; set; }

        public Command<SfPopupLayout> OpenPopupCommand { get; }

        public NewRewardBaseViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Images = new ObservableCollection<ImageInfo>();

            ImageSelectedCommand = new Command<ImageInfo>(OnImageSelected);
            OpenPopupCommand = new Command<SfPopupLayout>(OnOpenPopup);
            PageAppearingCommand = new AsyncCommand(OnAppearing);

        }

        void OnImageSelected(ImageInfo imageSource)
        {
            SelectedImage = imageSource.ImgSource;
            SelectedPath = imageSource.ImagePath;
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
            SelectedPath = defaultIcon;


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


        public string Text
        {
            get => text;
            set
            {
                SetProperty(ref text, value);
                if (string.IsNullOrWhiteSpace(text))
                {
                    HasNameError = true;
                }
                else
                    HasNameError = false;

            }
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Price
        {
            get => price;
            set
            {
                string cleanString = Helpers.Helpers.RemoveInvalidNumberInput(value);
                price = price + "x";
                SetProperty(ref price, cleanString);

                if (string.IsNullOrWhiteSpace(price) || int.Parse(price) == 0)
                {
                    HasPriceError = true;
                }
                else
                    HasPriceError = false;
            }
        }

        private bool hasPriceError;

        public bool HasPriceError
        {
            get => hasPriceError;
            set => SetProperty(ref hasPriceError, value);
        }

        private bool hasNameError;

        public bool HasNameError
        {
            get => hasNameError;
            set => SetProperty(ref hasNameError, value);
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

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(price);
        }

        public virtual async void OnSave()
        {

        }
    }
}
