using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Text;
using System.Threading.Tasks;
using Tinder_Admin.Controllers.Shared;
using Tinder_Admin.DTOs.Auth;
using Tinder_Admin.Entities;
using Tinder_Admin.Helpers;
using Tinder_Admin.Helpers.Constants;
using Tinder_Admin.Services.Interfaces;

namespace Tinder_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class AuthController : _APIController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IJWTService _jwtService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IJWTService jwtService,
            IWebHostEnvironment webHostEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _jwtService = jwtService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest("Invalid username or password.");
            }

            if (!user.Enabled)
            {
                return BadRequest("Account is locked.");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = await _jwtService.GenerateJWTTokenAsync(user);
                return Ok(new { token });
            }

            return BadRequest("Invalid username or password.");

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppUser user = new AppUser()
            {
                UserName = model.Email,
                Name = model.Name,
                Email = model.Email,
                Address = model.Address,
                Enabled = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleConstants.ENDUSER);
                var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                confirmEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmEmailToken));

                var confirmUrl = $"{AppConstants.URL}/confirm-email?userId={user.Id}&token={confirmEmailToken}";
                string body = string.Empty;
                var htmlFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Helpers/Htmls/send_mail.html");
                using (StreamReader reader = new StreamReader(htmlFilePath))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{redirectVerify}", confirmUrl);
                body = body.Replace("{UserName}", model.Email);

                bool isSendEmail = EmailHelper.SendVerifyEmail(model.Email ?? "", "Confirm your account", body, true);

                if (isSendEmail)
                {
                    return Ok("Verification email sent successfully.");
                }
                else
                {
                    return BadRequest("Failed to send verification email.");
                }
            }

            return BadRequest(result.Errors);
        }



        [Authorize(Roles = RoleConstants.SUPERADMIN)]
        [HttpPost("super-admin-register")]
        public async Task<IActionResult> SARegister([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Address = model.Address,
                Enabled = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(user, RoleConstants.ADMIN);
            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var principal = _jwtService.ValidateJWTTokenAsync(model.Token);
            if (principal == null)
            {
                return BadRequest("Invalid token.");
            }

            var newToken = await _jwtService.RefreshTokenAsync(model.Token);
            if (newToken == null)
            {
                return BadRequest("Unable to refresh token.");
            }

            return Ok(new { token = newToken });
        }

    }
}
