using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTodoApp.Views.General
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContextMenuView : Frame
    {
       /* public static readonly BindableProperty CommandProperty =
           BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContextMenuView), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { contextDeleteButton.Command = Command;  SetValue(CommandProperty, value); }
        }

        public static void Execute(ICommand command)
        {
            if (command == null) return;
            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }
        public Command OnTap => new Command(() => Execute(Command));*/
        public ContextMenuView()
        {
            InitializeComponent();
            

        }


    }
}