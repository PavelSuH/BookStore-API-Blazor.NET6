using System.ComponentModel.DataAnnotations;

namespace WebStore.API.Models.User
{
    public class UserDTO
    {   
        [Required]
        public string Email { get; set; }
        [Required]
        [EmailAddress]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
