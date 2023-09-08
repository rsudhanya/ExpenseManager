using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Common
{
    [ExcludeFromCodeCoverage]
    public class ExpenseManagerException : Exception
    {
        public ExpenseManagerException(string message)
            : base(message)
        {

        }

        public ExpenseManagerException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}
