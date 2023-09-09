using ExpenseManager.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExpenseManager.Api
{
    public class Utility
    {
        /// <summary>
        /// It is used to get user is from header
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Guid GetUserId(HttpContext httpContext)
        {
            // Get the Authorization header value
            string authorizationHeader = httpContext.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new ArgumentException(Constants.INVALID_AUTHORIZATION_TOKEN);
            }
            // Extract the JWT token from the Authorization header
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Parse the JWT token to retrieve claims
            var handler = new JwtSecurityTokenHandler();
            var jwtPayload = handler.ReadJwtToken(token);

            // Access specific claims by type
            var userIdClaim = jwtPayload.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new ArgumentException(Constants.INVALID_USERID);
            }
            return Guid.Parse(userIdClaim.Value);
        }
    }
}