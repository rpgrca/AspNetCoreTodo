using Microsoft.AspNetCore.Identity;

namespace TodoApi.Models {
    public class ApplicationUser : IdentityUser {
        public ApplicationUser() : base() {

        }
        
        public ApplicationUser(string name) : base(name) {

        }
    }
}