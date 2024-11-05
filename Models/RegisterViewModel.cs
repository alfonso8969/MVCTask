using System.ComponentModel.DataAnnotations;

namespace MVCTask.Models {
    public class RegisterViewModel {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(25, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [EmailAddress(ErrorMessage = "The field {0} is not a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
