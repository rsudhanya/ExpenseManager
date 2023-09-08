using ExpenseManager.Common;
using ExpenseManager.Common.Dtos;
using ExpenseManager.Common.Models;
using ExpenseManager.Service.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExpenseManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// It is used to login an user; Get an access token
        /// </summary>
        /// <param name="loginUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel loginUserModel)
        {
            try
            {
                _logger.LogInformation($"LoginAsync {Constants.METHOD_START}");

                if (loginUserModel is null)
                    throw new ArgumentNullException(Constants.DATA_NOT_PROVIDED);

                var response = await _authenticationService.AuthenticateUserAsync(loginUserModel);

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
                _logger.LogInformation($"LoginAsync {Constants.METHOD_END}");
            }
        }

        /// <summary>
        /// It is used to register an user; Create an user
        /// </summary>
        /// <param name="registerRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(RegisterResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserModel registerRequestModel)
        {
            try
            {
                _logger.LogInformation($"RegisterAsync {Constants.METHOD_START}");

                if (registerRequestModel is null)
                    throw new ArgumentNullException(Constants.DATA_NOT_PROVIDED);

                var response = await _authenticationService.RegisterUserAsync(registerRequestModel);

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
                _logger.LogInformation($"RegisterAsync {Constants.METHOD_END}");
            }
        }
    }
}
