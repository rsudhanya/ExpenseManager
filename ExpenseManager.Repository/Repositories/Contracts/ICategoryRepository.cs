using ExpenseManager.Repository.Entities;

namespace ExpenseManager.Repository.Repositories.Contracts
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// It is used to get the categories
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<Category>> GetCategoriesAsync(Guid userId);

        /// <summary>
        /// It is used to get category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Category> GetCategoryAsync(Guid categoryId, Guid userId);

        /// <summary>
        /// It is used to create a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task CreateCategoryAsync(Category category);

        /// <summary>
        /// It is used to create multiple categories
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        Task CreateCategoriesAsync(IEnumerable<Category> categories);

        /// <summary>
        /// It is used to update a category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateCategoryAsync(Category category, Guid userId);

        /// <summary>
        /// It is used to delete a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteCategoryAsync(Guid categoryId, Guid userId);
    }
}
