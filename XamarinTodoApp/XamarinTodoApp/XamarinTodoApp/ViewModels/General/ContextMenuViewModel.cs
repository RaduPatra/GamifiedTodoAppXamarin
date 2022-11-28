using System;
using System.Collections.Generic;
using System.Text;

//using MvvmHelpers.Commands;
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
//using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Linq;
using XamarinTodoApp.ViewModels.General;
using XamarinTodoApp.Resources;
using XamarinTodoApp.Helpers;
using XamarinTodoApp.Views;
using Google.Cloud.Firestore;
using Plugin.LocalNotification;
using XamarinTodoApp.Services.Interfaces;



namespace XamarinTodoApp.ViewModels.General
{
    public class ContextMenuViewModel<T> : ViewModelBase where T : IListItem
    {
        private bool isNavBarOn;
        public Command<object> LongPressCommand { get; }
        public Command<object> ContextMenuCommand { get; }
        public Command<T> TapSelectCommand { get; }

        public AsyncCommand DeleteSelectedCommand { get; }
        public Command<Frame> CloseContextMenuCommand { get; }

        public Func<T, Task> DeleteAction { get; set; }
        public Action<T> DefaultSelect { get; set; }

        public Action<Command<T>> ItemTapped { get; set; }


        /*public ContextMenuViewModel()
        {
            ContextMenuCommand = new Command<object>(OpenContextMenu);
            LongPressCommand = new Command<object>(OnLongPress);
            TapSelectCommand = new Command<TodoItemViewModel>(OnTapSelect);
            DeleteSelectedCommand = new AsyncCommand(OnDeleteSelected);
            CloseContextMenuCommand = new Command<Frame>(OnCloseContextMenu);
        }*/
        public ContextMenuViewModel()
        {

        }
        public ContextMenuViewModel(Action<Command<T>> itemTapped, Func<T, Task> deleteAction, Action<T> defaultSelect)
        {
            //deleteAction.Invoke();
            DeleteAction = deleteAction;
            DefaultSelect = defaultSelect;
            ItemTapped = itemTapped;


            ContextMenuCommand = new Command<object>(OpenContextMenu);
            LongPressCommand = new Command<object>(OnLongPress);
            TapSelectCommand = new Command<T>(OnTapSelect);
            DeleteSelectedCommand = new AsyncCommand(OnDeleteSelected);
            CloseContextMenuCommand = new Command<Frame>(OnCloseContextMenu);
            ItemsSelectedCount = 0;
            IsNavBarOn = true;

        }

        public bool IsNavBarOn
        {
            get => isNavBarOn;
            set => SetProperty(ref isNavBarOn, value);
        }

        private int itemsSelectedCount;
        public int ItemsSelectedCount
        {
            get => itemsSelectedCount;
            set
            {
                SetProperty(ref itemsSelectedCount, value);

            }
        }

        private Frame contextMenuFrame;
        private void OnCloseContextMenu(Frame frame)
        {
            frame.IsVisible = false;

            selectionMode = false;
            IsNavBarOn = true;
            ItemTapped(new Command<T>(DefaultSelect));
            //ItemTapped = new Command<TodoItemViewModel>(DefaultSelect);
            contextMenuFrame = null;
            foreach (var item in SelectedItems)
            {
                item.IsSelected = false;
            }
            SelectedItems.Clear();
        }

        private async Task OnDeleteSelected()
        {
            foreach (var item in SelectedItems)
            {
                await DeleteAction(item);
            }
            OnCloseContextMenu(contextMenuFrame);

        }

        bool selectionMode = false;
        public bool SelectionMode
        {
            get => selectionMode;
            set => SetProperty(ref selectionMode, value);
        }
        public ObservableCollection<T> SelectedItems { get; set; } = new ObservableCollection<T>();
        private void OnLongPress(object parameter)
        {
            var values = (object[])parameter;
            OpenContextMenu(parameter);
            var itemVM = (T)values[1];


            if (selectionMode) return;
            ItemTapped(TapSelectCommand);
            //ItemTapped = TapSelectCommand;
            itemVM.IsSelected = true;
            selectionMode = true;
            ItemsSelectedCount = 1;
            SelectedItems.Add(itemVM);


        }

        void OpenContextMenu(object parameter)
        {
            var values = (object[])parameter;

            var frame = values[0] as Frame;
            var fff = frame.BindingContext;
            //var deleteButton = values[1] as Button;
            var item = values[1] as TodoItemViewModel;
            frame.IsVisible = true;
            IsNavBarOn = false;
            contextMenuFrame = frame;


            //deleteButton.IsVisible = true;
            //deleteButton.CommandParameter = item;
        }

        private void OnTapSelect(T itemVM)
        {
            if (!selectionMode) return;

            if (!itemVM.IsSelected)
            {
                itemVM.IsSelected = true;
                SelectedItems.Add(itemVM);
                ItemsSelectedCount++;
            }
            else
            {
                itemVM.IsSelected = false;
                SelectedItems.Remove(itemVM);
                ItemsSelectedCount--;
            }

            if (SelectedItems.Count == 0)
            {
                OnCloseContextMenu(contextMenuFrame);
            }
        }

        public void OnAppearing()//call this in page onappearing
        {
            if (contextMenuFrame != null)
                OnCloseContextMenu(contextMenuFrame);
        }
    }
}