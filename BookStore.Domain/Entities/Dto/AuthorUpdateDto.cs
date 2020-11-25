using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Entities.Dto
{
    public class AuthorUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string Profile { get; set; }
    }
}