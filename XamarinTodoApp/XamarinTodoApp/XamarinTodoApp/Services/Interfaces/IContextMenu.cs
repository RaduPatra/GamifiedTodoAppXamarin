using System;
using System.Collections.Generic;
using System.Text;
using XamarinTodoApp.ViewModels.General;

namespace XamarinTodoApp.Services.Interfaces
{
    public interface IContextMenu<TListItem> where TListItem : IListItem
    {
        ContextMenuViewModel<TListItem> ContextMenuVM { get; set; }
    }
}
