using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models.ViewModels;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        // DOTNET FEATURE
        // 1. user manager service - create, register users to db
        // 2. sign in user
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginInfo)
        {
            Microsoft.AspNetCore.Identity.SignInResult? signInResult = await _signInManager.PasswordSignInAsync(loginInfo.UserName, loginInfo.Password, false, false);

            if (signInResult != null
                && signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
