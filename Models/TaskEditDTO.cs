using MVCTask.Entities;
using System.ComponentModel.DataAnnotations;

namespace MVCTask.Models {
#pragma warning disable S101 // Types should be named in PascalCase
    public class TaskEditDTO {

        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Step> Steps { get; set; }
    }
}
