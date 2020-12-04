using System.ComponentModel.DataAnnotations;

namespace BookStore.BlazorServer.Models
{
    public class UserModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 5)]
        public string Username { get; set; }
    }

    public class LoginModel : UserModel
    {
        public bool RememberMe { get; set; }
    }

    public class RegistrationModel : UserModel
    {
        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}