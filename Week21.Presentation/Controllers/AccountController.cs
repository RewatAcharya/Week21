using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Week21.Domain;
using Week21.Infrastructure;

namespace Week21.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser(Register model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser { UserName = model.Email, Email = model.Email };

            // Check if the specified role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                // If the role doesn't exist, return error
                return BadRequest("Invalid role specified.");
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign the specified role to the user
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok("User registered successfully.");
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users); 
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("Succeeded");
            }
            return BadRequest(result.Errors);
        }

        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile(AppUser appUser)
        {
            var user = await _userManager.FindByIdAsync(appUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.PhoneNumber = appUser.PhoneNumber;
            user.UserName = appUser.Email;
            user.Email = appUser.Email;
            user.NormalizedEmail = appUser.Email.ToUpper();
            user.EmailConfirmed = appUser.EmailConfirmed;
            
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            if (changePasswordResult.Succeeded)
            {
                return Ok(model);
            }

            return BadRequest(changePasswordResult.Errors);
        }
    }
}
