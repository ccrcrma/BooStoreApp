using System.Text.Json.Serialization;

namespace BooStoreApp.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        [JsonIgnore]
        public List<Book> BooksInGenre { get; set; }
    }
}
