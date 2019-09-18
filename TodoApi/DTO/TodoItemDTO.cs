using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTO {
    public class TodoItemDTO {
        public long Id {get;set;}
        [Required]
        public string Name {get; set;}
        [Required]
        public bool IsComplete {get;set;}
    }
}