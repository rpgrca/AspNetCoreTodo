using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTO {
    [DebuggerDisplay("{Name}")]
    public class GroupDTO {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}