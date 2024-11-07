using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCTask.Services {
    public static class Constants {
        public const string RolAdmin = "admin";
        public static readonly SelectListItem[] CulturesUISupported = [
            new SelectListItem("English", "en"),
            new SelectListItem("Español", "es")
            //new SelectListItem("Français", "fr"),
            //new SelectListItem("Русский", "ru"),
            //new SelectListItem("Українська", "uk")
        ];
    }
}
