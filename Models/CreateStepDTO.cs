using System.ComponentModel.DataAnnotations;

namespace MVCTask.Models {
#pragma warning disable S101 // Types should be named in PascalCase
    public class CreateStepDTO {

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
