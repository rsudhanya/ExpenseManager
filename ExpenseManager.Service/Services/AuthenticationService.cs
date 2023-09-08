using DnsClient.Internal;
using ExpenseManager.Common;
using ExpenseManager.Common.Dtos;
using ExpenseManager.Common.Models;
using ExpenseManager.Repository.Entities;
using ExpenseManager.Repository.Repositories.Contracts;
using ExpenseManager.Service.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace ExpenseManager.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        private readonly IConfiguration _configuration;

        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IUserRepository userRepository,

            IConfiguration configuration,
            
            ILogger<AuthenticationService> logger
            )
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); 

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// It is used to authenticate an user; Generates Jwt
        /// </summary>
        /// <param name="loginRequestModel"></param>
        /// <returns></returns>
        /// <exception cref="ExpenseManagerException"></exception>
        public async Task<LoginResponse> AuthenticateUserAsync(LoginRequestModel loginRequestModel)
        {
            _logger.LogInformation($"AuthenticateUserAsync {Constants.METHOD_START}");

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: GetUserAsync");
            var user = await _userRepository.GetUserAsync(loginRequestModel.Email);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: GetUserAsync");

            if (user is null)
            {
                throw new ExpenseManagerException(Constants.INVALID_CREDENTIALS);
            }

            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: CheckPasswordAsync");
            var isPasswordMatching = await _userRepository.CheckPasswordAsync(user, loginRequestModel.Password);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: CheckPasswordAsync");

            if (!isPasswordMatching)
            {
                throw new ExpenseManagerException(Constants.INVALID_CREDENTIALS);
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtSettings:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["jwtSettings:expiresInMinutes"]));

            var token = new JwtSecurityToken(
                    issuer: Dns.GetHostName(),
                    audience: Dns.GetHostName(),
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds);

            _logger.LogInformation($"AuthenticateUserAsync {Constants.METHOD_END}");

            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expires,
                Email = user.Email,
                UserId = user.Id
            };
        }

        /// <summary>
        /// It is used to create an user
        /// </summary>
        /// <param name="registerRequestModel"></param>
        /// <returns></returns>
        /// <exception cref="ExpenseManagerException"></exception>
        public async Task<RegisterResponse> RegisterUserAsync(RegisterUserModel registerRequestModel)
        {
            _logger.LogInformation($"RegisterUserAsync {Constants.METHOD_START}");


            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: GetUserAsync");
            var user = await _userRepository.GetUserAsync(registerRequestModel.Email);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: GetUserAsync");
            if (user is not null)
            {
                throw new ExpenseManagerException(Constants.USER_EXIST_EMAIL);
            }

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = registerRequestModel.Email,
                Email = registerRequestModel.Email,
            };
            _logger.LogInformation($"{Constants.CALLING_REPOSITORY}: CreateUserAsync");
            var result = await _userRepository.CreateUserAsync(applicationUser, registerRequestModel.Password);
            _logger.LogInformation($"{Constants.RECEIVED_DATA_REPOSITORY}: CreateUserAsync");

            if(!result.Succeeded)
            {
                var errorMessages = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorMessages.AppendLine(error.Description);
                }

                throw new ExpenseManagerException(errorMessages.ToString());
            }

            _logger.LogInformation($"RegisterUserAsync {Constants.METHOD_END}");

            return new RegisterResponse
            {
                UserId = applicationUser.Id
            };
        }
    }
}
