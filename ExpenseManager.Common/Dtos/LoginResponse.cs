using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Common.Dtos
{
    [ExcludeFromCodeCoverage]
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
