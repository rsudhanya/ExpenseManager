using ExpenseManager.Repository.DbContext.Contract;
using ExpenseManager.Repository.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Repository.DbContext
{
    [ExcludeFromCodeCoverage]
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var client = new MongoClient(configuration.GetConnectionString("databaseConnectionString"));
            _database = client.GetDatabase(configuration.GetConnectionString("databaseName"));
        }

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Expense> Expenses => _database.GetCollection<Expense>("Expenses");
    }
}
