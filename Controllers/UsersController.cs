using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCTask.Models;

namespace MVCTask.Controllers {
    public class UsersController: Controller {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            if(ModelState.IsValid) {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if(result.Succeeded) {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                foreach(var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
       }

    }
}
