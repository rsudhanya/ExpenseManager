using ExpenseManager.Common.Dtos;
using ExpenseManager.Common.Models;

namespace ExpenseManager.Service.Services.Contracts
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// It is used to authenticate an user; Generates Jwt
        /// </summary>
        /// <param name="loginRequestModel"></param>
        /// <returns></returns>
        Task<LoginResponse> AuthenticateUserAsync(LoginRequestModel loginRequestModel);

        /// <summary>
        /// It is used to create an user
        /// </summary>
        /// <param name="registerRequestModel"></param>
        /// <returns></returns>
        Task<RegisterResponse> RegisterUserAsync(RegisterUserModel registerRequestModel);
    }
}
