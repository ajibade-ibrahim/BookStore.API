using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Entities.Dto
{
    public class AuthorDto
    {
        public virtual ICollection<BookDto> Books { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string Profile { get; set; }
    }
}