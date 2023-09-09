using ExpenseManager.Common;
using ExpenseManager.Common.Dtos;
using ExpenseManager.Common.Models;
using ExpenseManager.Repository.Entities;
using ExpenseManager.Repository.Repositories.Contracts;
using Microsoft.Extensions.Logging;

namespace ExpenseManager.Service.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,


            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// It is used to get the categories
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(Guid userId)
        {
            _logger.LogInformation($"GetCategoriesAsync {Constants.METHOD_START}");

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: GetCategoriesAsync");
            var categories = await _categoryRepository.GetCategoriesAsync(userId);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: GetCategoriesAsync : categories.Count: {categories.Count}");

            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Description = c.Description
            });

            _logger.LogInformation($"GetCategoriesAsync {Constants.METHOD_END}");

            return categoryDtos;
        }

        /// <summary>
        /// It is used to create category
        /// </summary>
        /// <param name="categoryRequestModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CategoryDto> CreateCategoryAsync(
            CategoryRequestModel categoryRequestModel,
            Guid userId)
        {
            _logger.LogInformation($"CreateCategoryAsync {Constants.METHOD_START}");

            var category = new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = categoryRequestModel.CategoryName,
                Description = categoryRequestModel.Description,
                UserId = userId
            };

            var isAnyDuplicateCategory = await IsAnyDuplicateCategory(new List<Category> { category }, userId);
            if (isAnyDuplicateCategory)
            {
                throw new ExpenseManagerException(Constants.CATEGORY_EXIST);
            }

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: CreateCategoryAsync");
            await _categoryRepository.CreateCategoryAsync(category);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: CreateCategoryAsync");

            _logger.LogInformation($"CreateCategoryAsync {Constants.METHOD_END}");

            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
            };
        }

        /// <summary>
        /// It is used to create multiple category
        /// </summary>
        /// <param name="categoryRequestModels"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CategoryDto>> CreateCategoriesAsync(
            IEnumerable<CategoryRequestModel> categoryRequestModels,
            Guid userId)
        {
            _logger.LogInformation($"CreateCategoriesAsync {Constants.METHOD_START} categoryRequestModels.Count {categoryRequestModels.ToList().Count}");

            var categories = categoryRequestModels.Select(categoryRequestModel => new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = categoryRequestModel.CategoryName,
                Description = categoryRequestModel.Description,
                UserId = userId
            });

            var isAnyDuplicateCategory = await IsAnyDuplicateCategory(categories, userId);
            if (isAnyDuplicateCategory)
            {
                throw new ExpenseManagerException(Constants.CATEGORY_EXIST);
            }

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: CreateCategoriesAsync");
            await _categoryRepository.CreateCategoriesAsync(categories);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: CreateCategoriesAsync");

            _logger.LogInformation($"CreateCategoriesAsync {Constants.METHOD_END}");

            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
            });
        }

        /// <summary>
        /// It is used to update a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryRequestModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ExpenseManagerException"></exception>
        public async Task<CategoryDto> UpdateCategoryAsync(
            Guid categoryId,
            CategoryRequestModel categoryRequestModel,
            Guid userId)
        {
            _logger.LogInformation($"UpdateCategoryAsync {Constants.METHOD_START}");

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: GetCategoryAsync");
            var categoryfromDb = await _categoryRepository.GetCategoryAsync(categoryId, userId);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: GetCategoryAsync");

            if(categoryfromDb == null || categoryfromDb.Id != categoryId || categoryfromDb.UserId != userId)
            {
                throw new ExpenseManagerException(Constants.CATEGORY_NOT_FOUND);
            }

            var category = new Category
            {
                Id = categoryId,
                CategoryName = categoryRequestModel.CategoryName,
                Description = categoryRequestModel.Description,
                UserId = userId
            };

            var isAnyDuplicateCategory = await IsAnyDuplicateCategory(new List<Category> { category }, userId);
            if(isAnyDuplicateCategory)
            {
                throw new ExpenseManagerException(Constants.CATEGORY_EXIST);
            }

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: UpdateCategoryAsync");
            await _categoryRepository.UpdateCategoryAsync(category, userId);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: UpdateCategoryAsync");

            _logger.LogInformation($"UpdateCategoryAsync {Constants.METHOD_END}");

            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
            };
        }

        /// <summary>
        /// It is used to delete a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ExpenseManagerException"></exception>
        public async Task DeleteCategoryAsync(Guid categoryId, Guid userId)
        {
            _logger.LogInformation($"DeleteCategoryAsync {Constants.METHOD_START}");

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: GetCategoryAsync");
            var categoryfromDb = await _categoryRepository.GetCategoryAsync(categoryId, userId);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: GetCategoryAsync");

            if (categoryfromDb == null || categoryfromDb.Id != categoryId || categoryfromDb.UserId != userId)
            {
                throw new ExpenseManagerException(Constants.CATEGORY_NOT_FOUND);
            }

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: DeleteCategoryAsync");
            await _categoryRepository.DeleteCategoryAsync(categoryId, userId);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: DeleteCategoryAsync");

            _logger.LogInformation($"DeleteCategoryAsync {Constants.METHOD_END}");
        }

        /// <summary>
        /// It is used to check any new category exists or not
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<bool> IsAnyDuplicateCategory(IEnumerable<Category> categories, Guid userId)
        {
            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: GetCategoriesAsync");
            var categoriesFromDb = await _categoryRepository.GetCategoriesAsync(userId);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: GetCategoriesAsync : categoriesFromDb.Count: {categoriesFromDb.Count}");

            return categoriesFromDb.Any(cd => categories.Any(c => c.UserId == cd.UserId && c.CategoryName == cd.CategoryName));
        }
    }
}
