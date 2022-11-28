using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinTodoApp.Resources
{
    public static class Constants
    {

        public const string UserId = "userIdTemp";
        public const string FirebaseRefreshToken = "FirebaseRefreshToken";
        public const string WebApiKey = "AIzaSyDFVKP9U2pNDUntJgCZg23q2E0tbjr9ef0";
    }

    public static class MessageChannel
    {
        public const string UserXPChanged = "UserXPChanged";
        public const string UserCoinsChanged = "UserCoinsChanged";
        public const string UserAddAttribute = "UserAttributeChanged";
        public const string UserRemoveAttribute = "UserRemoveAttribute";
        public const string RefreshUserData = "UserAttributeChanged";
        public const string UpdateUserAttribute = "UpdateUserAttribute";

    }
}
