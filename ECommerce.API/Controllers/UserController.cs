using ECommerce.Entities;
using ECommerce.Services.Abstract;
using ECommerce.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService<ApplicationUser> _userService;

        public UserController(IUserService<ApplicationUser> userService)
        {
            _userService = userService;
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet, Route("[action]/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //[HttpPost, Route("[action]")]
        //public async Task<IActionResult> CreateUser(ApplicationUser user)
        //{
        //    var createdUser = await _userService.CreateUserAsync(user);
        //    return Ok(createdUser);
        //}

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> CreateUser(ApplicationUserDTO userDTO)
        {
            // {
            //   "userName": "kazim.dandir",
            //   "email": "kazim.dandir@example.com",
            //   "fullName": "Kazim Ikbal Dandir",
            //   "role": 0
            // }
            var user = new ApplicationUser
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                FullName = userDTO.FullName,
                Role = userDTO.Role
            };

            var createdUser = await _userService.CreateUserAsync(user);
            return Ok(createdUser);
        }

        //[HttpPut, Route("[action]")]
        //public async Task<IActionResult> UpdateUser(ApplicationUser user)
        //{
        //    var updatedUser = await _userService.UpdateUserAsync(user);
        //    return Ok(updatedUser);
        //}

        [HttpPut, Route("[action]")]
        public async Task<IActionResult> UpdateUser(string userId, ApplicationUserDTO userDTO)
        {
            // {
            //   "userName": "kazim.dandir",
            //   "email": "kazim.dandir@newdomain.com",
            //   "fullName": "Kazim Ikbal Dandir",
            //   "role": 1
            // }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = userDTO.Email;
            user.FullName = userDTO.FullName;
            user.Role = userDTO.Role;

            var updatedUser = await _userService.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete, Route("[action]/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var isDeleted = await _userService.DeleteUserAsync(userId);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
