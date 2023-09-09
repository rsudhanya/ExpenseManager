using ExpenseManager.Repository.Entities;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Repository.DbContext.Contract
{
    public interface IMongoDbContext
    {
        [ExcludeFromCodeCoverage]
        IMongoCollection<Category> Categories { get; }

        [ExcludeFromCodeCoverage]
        IMongoCollection<Expense> Expenses { get; }
    }
}
