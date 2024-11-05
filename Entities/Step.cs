﻿namespace MVCTask.Entities {
    public class Step {
        public Guid Id { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int Order { get; set; }
    }
}
