namespace CloneIntime.Utils.Constants
{
    public class ErrorMessage
    {
        public const string LoggedOutMessage = "Logged out";
        public const string IncorrectPasswordMessage = "Incorrect Password";
        public const string NotFoundMessage = "not found";

        public string GetNotFoundMessage(string entityName)
        {
            return $"{entityName} {NotFoundMessage}";
        }

    }
}
