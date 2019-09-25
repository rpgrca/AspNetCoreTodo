using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models {
    [DebuggerDisplay("{Name}")]
    public class TodoItem {
        public long Id { get;set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsComplete { get; set; }
        public User Responsible { get; set; }
    }
}