using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTO {
    [DebuggerDisplay("{Name}")]
    public class TodoItemDTO {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsComplete { get; set; }

        public long Order { get; set; }

        public string Description { get; set; }

        public DateTimeOffset DueAt { get; set; }
    }
}