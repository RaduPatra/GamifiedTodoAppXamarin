using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinTodoApp.Models;
namespace XamarinTodoApp.Services
{
    public interface ITodoService : IDataStore<TodoItem>
    {
       Task AddTodoAsync(TodoItem todoItem, string userId);

    }
}
