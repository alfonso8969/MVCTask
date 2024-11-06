using System.ComponentModel.DataAnnotations;

namespace MVCTask.Models {
    public class LoginViewModel {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(100, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe {
            get; set;
        }
    }

}
