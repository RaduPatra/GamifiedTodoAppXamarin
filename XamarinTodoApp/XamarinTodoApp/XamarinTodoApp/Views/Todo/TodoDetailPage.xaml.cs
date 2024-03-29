﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTodoApp.ViewModels;
using XamarinTodoApp.ViewModels.Todo;

namespace XamarinTodoApp.Views.Todo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoDetailPage : ContentPage
    {
        public TodoDetailPage()
        {
            InitializeComponent();
            BindingContext = new TodoDetailViewModel();
        }
    }
}