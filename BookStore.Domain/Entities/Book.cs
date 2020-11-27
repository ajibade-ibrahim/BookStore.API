using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public string ImageUrl { get; set; }

        [StringLength(50)]
        [Required]
        public string Isbn { get; set; }

        [Required]
        public double? Price { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Summary { get; set; }

        [Range(1700, 9999)]

        public int? Year { get; set; }
    }
}
