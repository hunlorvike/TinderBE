using System.Security.Claims;
using Tinder_Admin.Entities;

namespace Tinder_Admin.Services.Interfaces
{
    public interface IJWTService
    {
        Task<string> GenerateJWTTokenAsync(AppUser user);
        Task<ClaimsPrincipal> ValidateJWTTokenAsync(string token);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<string> RefreshTokenAsync(string token);
        Task<bool> IsTokenValidAsync(string token);
    }
}
