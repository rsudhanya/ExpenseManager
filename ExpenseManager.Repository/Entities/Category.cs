using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManager.Repository.Entities
{
    [ExcludeFromCodeCoverage]
    [CollectionName("Categories")]
    public class Category
    {
        [BsonId]
        public Guid Id { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public Guid UserId { get; set; }
    }
}
