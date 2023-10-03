namespace BooStoreApp.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public Genre? BookGenre { get; set; }
        public ICollection<AuthorBook>? AuthorBooks { get; set; }
    }
}
