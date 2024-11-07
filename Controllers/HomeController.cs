using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using MVCTask.Models;

namespace MVCTask.Controllers {
    public class HomeController: Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer) {
            _logger = logger;
            this.localizer = localizer;
        }

        public IActionResult Index() {
            ViewBag.Greet = localizer["Greet"] + "!";
            ViewBag.Know = localizer["UnKnow"] + "!";

            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChangeLanguage(string lang, string returnUrl) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            CultureInfo culture = new(lang);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            if(returnUrl is not null) {
                return LocalRedirect(returnUrl);
            } 
            return RedirectToAction("Index");
            
        }
    }
}
