using ExpenseManager.Repository.DbContext.Contract;
using ExpenseManager.Repository.Entities;
using ExpenseManager.Repository.Repositories.Contracts;
using MongoDB.Driver;

namespace ExpenseManager.Repository.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoDbContext _mongoDbContext;

        public CategoryRepository(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        /// <summary>
        /// It is used to get the categories
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<Category>> GetCategoriesAsync(Guid userId)
        {
            return await _mongoDbContext.Categories.Find(x => x.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// It is used to get category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Category> GetCategoryAsync(Guid categoryId, Guid userId)
        {
            return await _mongoDbContext.Categories.Find(c => c.Id == categoryId && c.UserId == userId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// It is used to create a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task CreateCategoryAsync(Category category)
        {
            await _mongoDbContext.Categories.InsertOneAsync(category);
        }

        /// <summary>
        /// It is used to create multiple categories
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        public async Task CreateCategoriesAsync(IEnumerable<Category> categories)
        {
            await _mongoDbContext.Categories.InsertManyAsync(categories);
        }

        /// <summary>
        /// It is used to update a category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateCategoryAsync(Category category, Guid userId)
        {
            await _mongoDbContext.Categories.ReplaceOneAsync(c => c.Id == category.Id && c.UserId == userId, category);
        }

        /// <summary>
        /// It is used to delete a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteCategoryAsync(Guid categoryId, Guid userId)
        {
            await _mongoDbContext.Categories.DeleteOneAsync(c => c.Id == categoryId && c.UserId == userId);
        }
    }
}
