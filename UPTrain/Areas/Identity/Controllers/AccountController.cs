using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UPTrain.Models;
using UPTrain.ViewModels;

namespace UPTrain.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User users = new User
                {
                    FullName = registerViewModel.FullName,
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email
                };

                var result = await userManager.CreateAsync(users, registerViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerViewModel);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return View(loginViewModel);
                }

                if (user.IsBlocked)
                {
                    ModelState.AddModelError("", "Your account has been blocked. Please contact support.");
                    return View(loginViewModel);
                }

                var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);

                if (result.Succeeded)
                {
                    
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName ?? user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()) 
            };

                    var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);

                    
                    if (user.Role == UserRole.Admin)
                        return RedirectToAction("Index", "Home", new { area = "Admin" });

                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                }
            }

            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}
