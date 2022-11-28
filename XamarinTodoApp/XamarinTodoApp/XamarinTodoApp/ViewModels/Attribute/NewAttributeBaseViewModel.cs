using System;
using System.Collections.Generic;
using System.Text;
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
    public class NewAttributeBaseViewModel : ViewModelBase
    {
        private string text;
        private string description;

        private SfPopupLayout currentPopup;
        private ImageSource selectedimage;
        //private string selectedPath;

        public string SelectedPath { get; set; }
        public ObservableCollection<ImageInfo> Images { get; set; }

        public Command<ImageInfo> ImageSelectedCommand { get; set; }

        public Command<SfPopupLayout> OpenPopupCommand { get; }

        public AsyncCommand PageAppearingCommand { get; }

        public NewAttributeBaseViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PageAppearingCommand = new AsyncCommand(OnAppearing);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            Images = new ObservableCollection<ImageInfo>();

            ImageSelectedCommand = new Command<ImageInfo>(OnImageSelected);
            OpenPopupCommand = new Command<SfPopupLayout>(OnOpenPopup);
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


        public ImageSource SelectedImage
        {
            get => selectedimage;
            set => SetProperty(ref selectedimage, value);
        }
        void InitImages()
        {
            //"XamarinTodoApp.Resources.AttributeIcons.default.png"
            Assembly assem = Assembly.GetExecutingAssembly();
            var folderName = "XamarinTodoApp.Resources.AttributeIcons";
            var fileNames = assem.GetManifestResourceNames().Where(r => r.StartsWith(folderName) && r.EndsWith(".png"));

            var defaultIcon = $"{folderName}.default.png";
            SelectedImage = ImageSource.FromResource(defaultIcon, typeof(NewAttributePage).GetTypeInfo().Assembly);
            SelectedPath = defaultIcon;


            foreach (var file in fileNames)
            {
                var imgSource = ImageSource.FromResource(file, typeof(NewAttributePage).GetTypeInfo().Assembly);
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
            return !String.IsNullOrWhiteSpace(text);
            //&& !String.IsNullOrWhiteSpace(description);
        }

        public string Text
        {
            get => text;
            set
            {
                HasNameError = false;
                SetProperty(ref text, value);
            }
        }


        private bool hasNameError;
        public bool HasNameError
        {
            get => hasNameError;
            set => SetProperty(ref hasNameError, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
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
        public virtual async void OnSave()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                HasNameError = true;
                return;
            }

        }
    }
}
