using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_Project.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Name { get; set; }
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; } 
        public User User { get; set; }
    }
}
