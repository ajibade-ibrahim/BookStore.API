using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Entities.Dto
{
    public class BookUpdateDto
    {
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