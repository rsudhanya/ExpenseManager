using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ExpenseManager.Repository.Entities
{
    [ExcludeFromCodeCoverage]
    [CollectionName("Expenses")]
    public class Expense
    {
        [BsonId]
        public Guid Id { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public string Statement { get; set; }

        public Guid CategoryId { get; set; }

        public decimal Amount { get; set; }

        public Guid UserId { get; set; }

        public string SubExpenseName { get; set; }
    }
}
