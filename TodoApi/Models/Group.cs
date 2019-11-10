using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TodoApi.Models {
    [DebuggerDisplay("{Name}")]
    public class Group {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<ApplicationUser> Members { get; set; }
    }
}