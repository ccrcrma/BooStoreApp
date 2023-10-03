using BooStoreApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BooStoreApp.DTO
{
    public class BookDTO
    {
        [Required]
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public int GenreId { get; set; }
    }
}
