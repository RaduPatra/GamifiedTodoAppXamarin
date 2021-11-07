using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services
{
    public class FirestoreDatabase<T> : IDataStore<T>
        where T : IDataBaseItem
    {
        private const string BaseUrl = "https://xamarintodoapp-default-rtdb.europe-west1.firebasedatabase.app/";
        private const string projectId = "xamarintodoapp";

        private readonly CollectionReference collRef;

        FirestoreDb db;


        public FirestoreDatabase(string path)
        {
            db = FirestoreDb.Create(projectId);
            collRef = db.Collection(path);

        }
        public async Task<bool> AddItemAsync(T item)
        {
            try
            {
                var itemId = collRef.Document().Id;
                item.Id = itemId;
                DocumentReference docRef = collRef.Document(item.Id);
                await docRef.SetAsync(item);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateItemAsync(string id, T item)
        {
            try
            {
                DocumentReference docRef = collRef.Document(id);
                await docRef.SetAsync(item);
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
                DocumentReference docRef = collRef.Document(id);
                await docRef.DeleteAsync();

            }
            catch (Exception)
            {
                return false;
            }

            return await Task.FromResult(true);
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                DocumentReference docRef = collRef.Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    T item = snapshot.ConvertTo<T>();
                    return item;
                }
                return default;
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
                List<T> itemsToReturn = new List<T>();
                QuerySnapshot allItems = await collRef.GetSnapshotAsync();
                foreach (DocumentSnapshot document in allItems.Documents)
                {
                    if (!document.Exists) continue;
                    T item = document.ConvertTo<T>();
                    itemsToReturn.Add(item);
                }
                return itemsToReturn;
            }
            catch (Exception)
            {
                return default;
            }
        }

    }
}
