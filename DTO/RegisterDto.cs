using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DTO
{
    public class RegisterDto
    { 
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string ConfirmPassword { get; set; }


        public bool IsAdmin { get; set; }=false;


    }
}