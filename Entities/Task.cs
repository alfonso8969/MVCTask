using System.ComponentModel.DataAnnotations;

namespace MVCTask.Entities {
    public class Task {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }

        public List<Step> Steps { get; set; }
        public List<FilesAttach> FilesAttach { get; set; }


    }
}
