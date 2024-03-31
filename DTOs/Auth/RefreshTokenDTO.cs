using System.ComponentModel.DataAnnotations;

namespace Tinder_Admin.DTOs.Auth
{
    public class RefreshTokenDTO
    {
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; }
    }
}
