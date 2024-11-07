using System.ComponentModel.DataAnnotations;

namespace MVCTask.Models {
    public class LoginViewModel {
        [Required(ErrorMessage = "Error.Required")]
        [StringLength(20, ErrorMessage = "StringLength", MinimumLength = 3)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Error.Required")]
        [StringLength(100, ErrorMessage = "StringLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "RememberMe")]
        public bool RememberMe {
            get; set;
        }
    }

}
