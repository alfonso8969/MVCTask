using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCTask.Models;
using MVCTask.Services;
using System.Security.Claims;

namespace MVCTask.Controllers {
    public class UsersController: Controller {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;

        public UsersController(UserManager<IdentityUser> userManager,
                                           SignInManager<IdentityUser> signInManager,
                                           ApplicationDbContext context) {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null, string message = null) {
            if (ModelState.IsValid) {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            if (message is not null) {
                ViewData["Message"] = message;
            }

            return View(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid) {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded) {
                    return RedirectToAction("Index", "Home");
                } else {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ChallengeResult ExternalLogin(string provider, string returnUrl) {
            if (!ModelState.IsValid) {
                return new ChallengeResult(provider, new AuthenticationProperties());
            }
            var redirectUrl = Url.Action("ExternalLoginCallback", "Users", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null) {
            if (!ModelState.IsValid) {
                return View("Login");
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var message = "";
            if (remoteError is not null) {
                message = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login", routeValues: new { message });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info is null) {
                message = "Error loading external login information.";
                return RedirectToAction("Login", routeValues: new { message });
            }

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            if (result.Succeeded) {
                return LocalRedirect(returnUrl);
            }

            string email = "";
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)) {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            } else {
                message = "Email claim not found from external provider.";
                return RedirectToAction("Login", routeValues: new { message });
            }

            var user = new IdentityUser { UserName = email, Email = email };

            var createResult = await userManager.CreateAsync(user);

            if (createResult.Succeeded) {
                createResult = await userManager.AddLoginAsync(user, info);
                if (createResult.Succeeded) {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return LocalRedirect(returnUrl);
                }
            } else {
                message = createResult.Errors.First().Description;
                return RedirectToAction("Login", routeValues: new { message });
            }

            var resultAddLogin = await userManager.AddLoginAsync(user, info);
            if (resultAddLogin.Succeeded) {
                await signInManager.SignInAsync(user, isPersistent: true, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            message = "A error occurred while adding the login to the user.";
            return RedirectToAction("Login", routeValues: new { message });
        }

        [HttpGet]
        [Authorize(Roles = Constants.RolAdmin)]
        public async Task<IActionResult> ListUsers(string message = "") {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var users = await context.Users.Select(u => new UserViewModel {
                Email = u.Email
            }).ToListAsync();

            var model = new UsersListViewModel {
                Users = users,
                Message = message
            };

            return View("list", model);
        }

        [Authorize(Roles = Constants.RolAdmin)]
        public async Task<IActionResult> DoRemoveFromAdmin(string email) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null) {
                return NotFound();
            }
            var isAdmin = await userManager.IsInRoleAsync(user, Constants.RolAdmin);
            if (!isAdmin) {
                await userManager.AddToRoleAsync(user, Constants.RolAdmin);
                return RedirectToAction("ListUsers", new { message = $"User {email} is now an admin." });
            } else {
                await userManager.RemoveFromRoleAsync(user, Constants.RolAdmin);
                return RedirectToAction("ListUsers", new { message = $"User {email} is no longer an admin." });
            }
        }
    }
}
