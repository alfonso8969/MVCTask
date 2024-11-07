using System.ComponentModel.DataAnnotations;

namespace MVCTask.Models {
    public class RegisterViewModel {
        [Required(ErrorMessage = "Error.Required")]
        [StringLength(25, ErrorMessage = "StringLength", MinimumLength = 3)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Error.Required")]
        [EmailAddress(ErrorMessage = "EmailAddress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Error.Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Error.Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "PasWordNotMatch")]
        public string ConfirmPassword { get; set; }
    }
}
