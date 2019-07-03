using System;

namespace AspNetCoreTodo.Models {
    public class TodoViewModel {
        public TodoItem[] Items { get; set; }

        public TodoViewModel() {
            this.Items = new TodoItem[] { new TodoItem() { Title = "Ver partido", Id = Guid.NewGuid(), IsDone = false } };
        }
    }
}