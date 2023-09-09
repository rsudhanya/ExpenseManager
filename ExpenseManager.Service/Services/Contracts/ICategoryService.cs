using ExpenseManager.Common.Dtos;
using ExpenseManager.Common.Models;

namespace ExpenseManager.Service.Services.Contracts
{
    public interface ICategoryService
    {
        /// <summary>
        /// It is used to get the categories
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync(Guid userId);

        /// <summary>
        /// It is used to create category
        /// </summary>
        /// <param name="categoryRequestModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CategoryDto> CreateCategoryAsync(
            CategoryRequestModel categoryRequestModel,
            Guid userId);

        /// <summary>
        /// It is used to create multiple category
        /// </summary>
        /// <param name="categoryRequestModels"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<CategoryDto>> CreateCategoriesAsync(
            IEnumerable<CategoryRequestModel> categoryRequestModels,
            Guid userId);

        /// <summary>
        /// It is used to update a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryRequestModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ExpenseManagerException"></exception>
        Task<CategoryDto> UpdateCategoryAsync(
            Guid categoryId,
            CategoryRequestModel categoryRequestModel,
            Guid userId);

        /// <summary>
        /// It is used to delete a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ExpenseManagerException"></exception>
        Task DeleteCategoryAsync(Guid categoryId, Guid userId);
    }
}
