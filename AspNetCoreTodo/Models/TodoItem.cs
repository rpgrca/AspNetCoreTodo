using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models {
    public class TodoItem {
        public TodoItem() {
            this.Id = Guid.NewGuid();
            this.IsDone = false;
            this.DueAt = null;
        }
        public TodoItem(string title) : base() {
            this.Title = title;
        }

        public Guid Id { get; set; }

        public bool IsDone { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTimeOffset? DueAt { get; set; }

        public string UserId { get; set; }
    }
}