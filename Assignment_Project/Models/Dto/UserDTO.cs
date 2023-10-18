using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Assignment_Project.Dtos
{
    public class UserDTO
    {
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [JsonIgnore]
        public List<CompanyDTO> Companies { get; set; }

    }


}
