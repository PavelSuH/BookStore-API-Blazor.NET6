using System.ComponentModel.DataAnnotations;

namespace WebStore.API.Models.Book
{
    public class BookDetailsDTO : BaseDTO
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [Range(1800, int.MaxValue)]
        public int Year { get; set; }
        [Required]
        public string Isbn { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Summary { get; set; }
        public string Image { get; set; }
        [Required]
        public decimal Price { get; set; }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
