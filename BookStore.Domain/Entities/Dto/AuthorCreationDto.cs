using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Entities.Dto
{
    public class AuthorCreationDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string Profile { get; set; }
    }
}