using System.Text.Json.Serialization;

namespace BooStoreApp.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime DOB { get; set; }
        [JsonIgnore]
        public ICollection<AuthorBook> AuthorBooks { get; set; }
    }
}
