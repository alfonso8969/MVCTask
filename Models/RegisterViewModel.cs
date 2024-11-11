using System.ComponentModel.DataAnnotations;

namespace MVCTask.Models {
    public class RegisterViewModel {
        [Required(ErrorMessage = "Error.Required")]
        [StringLength(25, ErrorMessage = "StringLength", MinimumLength = 3)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Error.Required")]
        [EmailAddress(ErrorMessage = "EmailAddress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Error.Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Error.Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "PasWordNotMatch")]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
