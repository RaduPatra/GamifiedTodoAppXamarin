using System;
using System.Threading.Tasks;

namespace XamarinTodoApp.Services
{
    public interface IAuthService
    {
        Task<Exception> CreateUser(string email, string password);
        Task<Exception> Login(string email, string password);
        Task RefreshUserToken();
    }
}