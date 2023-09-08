using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Common.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponse
    {
        public string ErrorMessage { get; set; }
    }
}
