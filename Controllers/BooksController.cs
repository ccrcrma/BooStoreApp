using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooStoreApp.Data;
using BooStoreApp.Models;
using BooStoreApp.Utils;
using BooStoreApp.DTO;
using Microsoft.Data.SqlClient;

namespace BooStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public BooksController(BookStoreDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] PaginationFilter filter)
        {
            if(_context.Books == null)
          {
              return NotFound();
          }

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var data =await  _context.Books
                .Include(b => b.BookGenre)
                .Include(b => b.AuthorBooks)
                    .ThenInclude(ab => ab.Author)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .AsNoTracking()
                .ToListAsync();

            var totalRecords = await _context.Books.CountAsync();
            return Ok(new PagedResponse<List<Book>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords));
           

        }

        
        
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody]BookDTO bookdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var book = new Book()
            {
                Title = bookdto.Title,
                PublishedDate = bookdto.PublishedDate
            };
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == bookdto.GenreId);
            if (genre == null)
                return BadRequest(new { IsSucess = false, message = "Please  enter appropriate genre" });
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Ok(book);
        }


        [HttpGet]
        [Route("filtered")]
        public async Task<IActionResult> GetBooksFiltered([FromQuery] BookFilter filter)
        {
            var param1 = filter.Genre;
            var param2 = filter.Author;

            var books =   _context.Books
                .FromSqlRaw($"EXECUTE dbo.GetFilteredBlogs @genre, @author", 
                new SqlParameter("@genre",  string.IsNullOrEmpty(param1) ? DBNull.Value:  param1 ),
                new SqlParameter("@author", string.IsNullOrEmpty(param2) ? DBNull.Value : param2))
                .ToList();
            return Ok(books);
        }


        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(new Response<Book>(book));
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

    }
}
