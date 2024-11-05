using Microsoft.EntityFrameworkCore;

namespace MVCTask.Entities {
    public class FilesAttach {
        public Guid Id { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
        [Unicode]
        public string Url  { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
