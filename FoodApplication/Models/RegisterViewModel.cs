using System.ComponentModel.DataAnnotations;
namespace FoodApplication.Models
{
    public class RegisterViewModel
    {
        [Required,EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
