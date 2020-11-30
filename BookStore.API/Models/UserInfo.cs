using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public class UserInfo
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}