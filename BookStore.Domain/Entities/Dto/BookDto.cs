using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Entities.Dto
{
    public class BookDto
    {
        public virtual AuthorDto Author { get; set; }

        public Guid AuthorId { get; set; }
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }

        [StringLength(50)]
        [Required]
        public string Isbn { get; set; }

        [Required]
        public double? Price { get; set; }
        public string Summary { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [MaxLength(4)]
        public int? Year { get; set; }
    }
}