using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Common
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        #region ExceptionMessages

        //Authentication
        public const string INVALID_CREDENTIALS = "Invalid credentials. Email/Password mismatch or do not exist.";
        public const string USER_EXIST_EMAIL = "The user already exists with this email";
        public const string INVALID_AUTHORIZATION_TOKEN = "Unable to fetch authorization header or bearer token";
        public const string INVALID_USERID = "Invalid user id";

        //Category
        public const string CATEGORY_NOT_FOUND = "Category not found; Invalid categoryId or userId";
        public const string CATEGORY_EXIST = "Categories already exist";

        //Generic
        public const string DATA_NOT_PROVIDED = "Data not provided";
        public const string SERVER_ERROR = "Internal server error";
        public const string METHOD_START = "Method start";
        public const string METHOD_END = "Method end";
        public const string CALLING_REPOSITORY = "Calling repository";
        public const string RECEIVED_DATA_REPOSITORY = "Received data repository";
        #endregion
    }
}
