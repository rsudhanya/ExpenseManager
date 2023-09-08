using ExpenseManager.Repository.Entities;
using Microsoft.AspNetCore.Identity;

namespace ExpenseManager.Repository.Repositories.Contracts
{
    public interface IUserRepository
    {
        /// <summary>
        /// It is used to get an user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<ApplicationUser> GetUserAsync(string email);

        /// <summary>
        /// It is used to validate the password 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        /// <summary>
        /// It is used to create an user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    }
}
