using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models.ViewModels;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        // DOTNET FEATURE = user manager service - create, register users to db
        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAccountRequest register)
        {
            IdentityUser identityUser = new IdentityUser()
            {
                UserName = register.UserName,
                Email = register.Email,
            };

            IdentityResult userIdentityResult = await _userManager.CreateAsync(identityUser, register.Password);

            if (userIdentityResult.Succeeded)
            {
                // assign user a role(s)
                IdentityResult roleIdentityUserResult = await _userManager.AddToRoleAsync(identityUser, "User");

                if (roleIdentityUserResult.Succeeded)
                {
                    // show success notification
                    return RedirectToAction("Register");
                }
            }

            return View("Register");
        }
    }
}
