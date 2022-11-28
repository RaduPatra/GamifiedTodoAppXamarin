using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Core;
using Xamarin.Forms;
using XamarinTodoApp.Models;

namespace XamarinTodoApp.Services
{
    public class FirestoreDatabase<T> : IDataStore<T>
        where T : IDataBaseItem
    {
        private const string BaseUrl = "https://xamarintodoapp-default-rtdb.europe-west1.firebasedatabase.app/";
        private const string projectId = "xamarintodoapp";

        private AuthService AuthService => DependencyService.Get<AuthService>();



        protected readonly CollectionReference collRef;

        protected FirestoreDb db;

        /*string userId = AuthService.UserInfo.LocalId;
            string userUrl = $"{BaseUrl}users/{userId}/";*/
        public FirestoreDatabase(string path)
        {
            db = FirestoreDb.Create(projectId);

            collRef = db.Collection(path);

        }

        public FirestoreDb CreateFirestoreDbWithEmailAuthentication(string emailAddress, string password, string firebaseApiKey, string firebaseProjectId)
        {
            // Create a custom authentication mechanism for Email/Password authentication
            // If the authentication is successful, we will get back the current authentication token and the refresh token
            // The authentication expires every hour, so we need to use the obtained refresh token to obtain a new authentication token as the previous one expires
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey));
            var auth = authProvider.SignInWithEmailAndPasswordAsync(emailAddress, password).Result;
            var callCredentials = CallCredentials.FromInterceptor(async (context, metadata) =>
            {
                if (auth.IsExpired()) auth = await auth.GetFreshAuthAsync();
                if (string.IsNullOrEmpty(auth.FirebaseToken)) return;

                metadata.Clear();
                metadata.Add("authorization", $"Bearer {auth.FirebaseToken}");
            });
            var credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials);

            // Create a custom Firestore Client using custom credentials
            var grpcChannel = new Channel("firestore.googleapis.com", credentials);

            var firestoreClient = new FirestoreClientBuilder
            {
                CallInvoker = grpcChannel.CreateCallInvoker(),
                Settings = FirestoreSettings.GetDefault()
            }.Build();

            return FirestoreDb.Create(firebaseProjectId, firestoreClient);
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
                // id = "3hohYTtPOjYEZNE7WIIL8gnDXSa2";
                DocumentReference docRef = collRef.Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    T item = snapshot.ConvertTo<T>();
                    return item;
                }
                return default;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return default;
            }

        }

        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                var authService = DependencyService.Get<AuthService>();
                var userDocRef = db.Collection("users").Document(authService.UserInfo.LocalId);
                //var a = db.Collection("users").qu
                Query query = collRef.WhereEqualTo("CreatedBy", userDocRef);
                List<T> itemsToReturn = new List<T>();
                //QuerySnapshot allItems = await collRef.GetSnapshotAsync();
                QuerySnapshot userItems = await query.GetSnapshotAsync();
                foreach (DocumentSnapshot document in userItems.Documents)
                {
                    if (!document.Exists) continue;
                    T item = document.ConvertTo<T>();
                    itemsToReturn.Add(item);
                }
                return itemsToReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task<bool> AddTestAsync(T item)
        {
            try
            {
                await collRef.AddAsync(item);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
