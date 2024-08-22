using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Entities;
using ECommerce.UI.Models.Account;
using System.Threading.Tasks;
namespace ECommerce.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PasswordHash = model.Password };

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);


                if (result.Succeeded)
                {
                    return new JsonResult(new { success = true, message = "Login successful" });
                } 
            }

            // Model state errors to JSON
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {            

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FullName = model.FullName,
                    UserName = model.Email,
                    Email = model.Email,
                    Role = UserRole.Customer // We automatically set UserRole to Customer
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return new JsonResult(new { success = true, message = "Registration successful" });
                }

                var errors = result.Errors.Select(e => e.Description).ToList();
                return Json(new { success = false, errors });
            }

            // Model state errors to JSON
            var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            return Json(new { success = false, errors = modelErrors });
        }

        [HttpPost]
        public async Task<JsonResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new JsonResult(new { success = true, message = "Logged out successfully" });
        }
    }
}
