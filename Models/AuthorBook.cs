using System.Text.Json.Serialization;

namespace BooStoreApp.Models
{
    public class AuthorBook
    {
        [JsonIgnore]
        public int AuthorId { get; set; }
        
        public Author Author { get; set; }
        [JsonIgnore]
        public int BookId { get; set; }
        [JsonIgnore]
        public Book Book { get; set; }
    }
}
