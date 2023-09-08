using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Common.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RegisterResponse
    {
        public Guid UserId { get; set; }
    }
}
