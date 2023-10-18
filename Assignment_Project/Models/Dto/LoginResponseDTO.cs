using Assignment_Project.Dtos;

namespace Assignment_Project.Models.Dto
{
    public class LoginResponseDTO
    {
        public ApplicationUserDTO ApplicationUser { get; set; }
        public string Token { get; set; }
    }
}
