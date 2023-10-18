using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Assignment_Project.Dtos
{
        public class CompanyDTO
        {
            public int CompanyId { get; set; }
            [Required]
            [MaxLength(50)]
            public string Name { get; set; }
            public string Address { get; set; }
            [Required]
            public string Email { get; set; }
            public int UserId { get; set; }
            [JsonIgnore]
            public UserDTO User { get; set; }
        }

}
