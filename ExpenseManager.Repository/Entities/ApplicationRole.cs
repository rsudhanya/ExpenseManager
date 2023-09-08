using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Repository.Entities
{
    [ExcludeFromCodeCoverage]
    [CollectionName("Roles")]
    public class ApplicationRole : MongoIdentityRole<Guid>
    {

    }
}
