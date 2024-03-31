using System.ComponentModel.DataAnnotations;

namespace Tinder_Admin.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

    }
}
