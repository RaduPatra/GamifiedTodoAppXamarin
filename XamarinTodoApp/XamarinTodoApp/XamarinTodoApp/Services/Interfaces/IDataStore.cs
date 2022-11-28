using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace XamarinTodoApp.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(string id, T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
