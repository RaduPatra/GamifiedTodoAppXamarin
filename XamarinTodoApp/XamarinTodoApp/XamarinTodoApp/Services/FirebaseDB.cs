using Firebase.Database;
using Firebase.Database.Streaming;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinTodoApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace XamarinTodoApp.Services
{
    public class FirebaseDB<T> : IDataStore<T>
        where T : IDataBaseItem
    {
        private const string BaseUrl = "https://xamarintodoapp-default-rtdb.europe-west1.firebasedatabase.app/";

        private readonly ChildQuery _query;


        public readonly List<T> items;


        public FirebaseDB(string path)
        {
            _query = new FirebaseClient(BaseUrl).Child(path);
            
        }
        public async Task<bool> AddItemAsync(T item)
        {
            try
            {
                /*await _query
                    .PostAsync(item);*/

                await _query.Child(item.Id)
                    .PutAsync(item);


            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateItemAsync(string id, T item)
        {
            try
            {
                await _query
                    .Child(id)
                    .PutAsync(item);
            }
            catch (Exception)
            {
                return false;
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {

                await _query
                    .Child(id)
                    .DeleteAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Console.WriteLine(" GetItemAsync: " + id);

                return await _query
                    .Child(id)
                    .OnceSingleAsync<T>();
            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                var firebaseObjects = await _query
                    .OnceAsync<T>();

                return firebaseObjects
                    .Select(x => x.Object);
            }
            catch (Exception)
            {
                return default;
            }
        }

    }
}


