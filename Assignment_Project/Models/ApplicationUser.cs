using Microsoft.AspNetCore.Identity;

namespace Assignment_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
       

    }
}
