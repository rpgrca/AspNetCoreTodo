using System;

namespace AspNetCoreTodo.Models {
    public class ManageUsersViewModel {
         public ApplicationUser[] Administrators { get; set; }
         public ApplicationUser[] Everyone { get; set; }
    }
}