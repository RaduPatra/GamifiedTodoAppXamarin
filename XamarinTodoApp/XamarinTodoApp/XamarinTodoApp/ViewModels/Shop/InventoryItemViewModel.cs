using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.ViewModels.Shop
{
    public class InventoryItemViewModel : ObservableObject
    {
        private string quantity;
        private RewardItemViewModel rewardItemVM;

        public InventoryItemViewModel(InventoryItem item)
        {
            Item = item;
            rewardItemVM = new RewardItemViewModel(item.RewardItem);
            quantity = item.Quantity.ToString();
        }

        public string Quantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

        public RewardItemViewModel RewardItemVM
        {
            get => rewardItemVM;
            set => SetProperty(ref rewardItemVM, value);
        }


        public InventoryItem Item { get; set; }
    }
}
