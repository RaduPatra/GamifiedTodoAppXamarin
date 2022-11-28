using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace XamarinTodoApp.Services.Interfaces
{
    public interface IEnvironment
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint);
        void SetNavigationBarColor(Color color);
    }
}
