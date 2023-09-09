using ExpenseManager.Common;
using ExpenseManager.Common.Dtos;
using ExpenseManager.Common.Models;
using ExpenseManager.Service.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpenseManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        private readonly ILogger<CategoriesController> _logger;

        private readonly Utility _utility;

        public CategoriesController(
            ICategoryService categoryService, 

            ILogger<CategoriesController> logger,

            Utility utility)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _utility = utility ?? throw new ArgumentNullException(nameof(utility));
        }

        /// <summary>
        /// It is used to get categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            try
            {
                _logger.LogInformation($"GetCategoriesAsync {Constants.METHOD_START}");

                var userId = _utility.GetUserId(HttpContext);

                var response = await _categoryService.GetCategoriesAsync(userId);

                return Ok(response);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (ExpenseManagerException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            finally
            {
                _logger.LogInformation($"GetCategoriesAsync {Constants.METHOD_END}");
            }
        }

        /// <summary>
        /// It is used to create a category
        /// </summary>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        [Route("/")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequestModel categoryModel)
        {
            try
            {
                _logger.LogInformation($"CreateCategoryAsync {Constants.METHOD_START}");

                if(categoryModel is null)
                {
                    throw new ArgumentNullException(nameof(categoryModel));
                }

                var userId = _utility.GetUserId(HttpContext);

                var response = await _categoryService.CreateCategoryAsync(categoryModel,userId);

                return Ok(response);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (ExpenseManagerException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            finally
            {
                _logger.LogInformation($"CreateCategoryAsync {Constants.METHOD_END}");
            }
        }

        /// <summary>
        /// It is used to create multiple category
        /// </summary>
        /// <param name="categoryModels"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        [Route("categories")]
        public async Task<IActionResult> CreateCategoriesAsync([FromBody] IEnumerable<CategoryRequestModel> categoryModels)
        {
            try
            {
                _logger.LogInformation($"CreateCategoriesAsync {Constants.METHOD_START}");

                if (categoryModels is null || !categoryModels.Any())
                {
                    throw new ArgumentException(nameof(categoryModels));
                }

                var userId = _utility.GetUserId(HttpContext);

                var response = await _categoryService.CreateCategoriesAsync(categoryModels, userId);

                return Ok(response);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (ExpenseManagerException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            finally
            {
                _logger.LogInformation($"CreateCategoriesAsync {Constants.METHOD_END}");
            }
        }

        /// <summary>
        /// It is used to update a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryModel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPut]
        [Route("/{categoryId}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromRoute] [Required] Guid categoryId, [FromBody] CategoryRequestModel categoryModel)
        {
            try
            {
                _logger.LogInformation($"UpdateCategoryAsync {Constants.METHOD_START}");

                if(categoryId == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(categoryId));
                }
                if (categoryModel is null)
                {
                    throw new ArgumentNullException(nameof(categoryModel));
                }

                var userId = _utility.GetUserId(HttpContext);

                var response = await _categoryService.UpdateCategoryAsync(categoryId, categoryModel, userId);

                return Ok(response);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (ExpenseManagerException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            finally
            {
                _logger.LogInformation($"UpdateCategoryAsync {Constants.METHOD_END}");
            }
        }

        /// <summary>
        /// It is used to delete a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPut]
        [Route("/{categoryId}")]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute][Required] Guid categoryId)
        {
            try
            {
                _logger.LogInformation($"DeleteCategoryAsync {Constants.METHOD_START}");

                if (categoryId == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(categoryId));
                }

                var userId = _utility.GetUserId(HttpContext);

                await _categoryService.DeleteCategoryAsync(categoryId, userId);

                return Ok(true);
            }
            catch (ArgumentException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (ExpenseManagerException exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return BadRequest(error);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                var error = new ErrorResponse
                {
                    ErrorMessage = exception.Message
                };
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            finally
            {
                _logger.LogInformation($"DeleteCategoryAsync {Constants.METHOD_END}");
            }
        }
    }
}
