using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.API.Data;
using WebStore.API.Models.Author;
using WebStore.API.Models.Book;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class BooksController : ControllerBase
    {
        private BookStoreDbContext _context;
        private IMapper _mapper;
        private ILogger<BooksController> _logger;
        public BooksController(BookStoreDbContext context, IMapper mapper, ILogger<BooksController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/<BooksController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDTO>>> GetAllBoks()
        {
            var booksList = await _context
                .Books.Include(c => c.Author)
                .ProjectTo<BookReadOnlyDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            var viewList = _mapper.Map<IEnumerable<BookReadOnlyDTO>>(booksList);
            return Ok(viewList);
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult <BookDetailsDTO>> GetBook(int id)
        {
            var book = await _context.Books.Include(c => c.Author)
                .ProjectTo<BookDetailsDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == id);
            var bookDTO = _mapper.Map<BookDetailsDTO>(book);
            if(book == null)
            {
                return NotFound();
            }
            return Ok(bookDTO);
        }

        // POST api/<BooksController>
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateBook(BookCreateDTO bookDTo)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var books = await _context.Books.ToListAsync();
            var book = _mapper.Map<Book>(bookDTo);
            books.Add(book);
            _context.SaveChanges();
            return CreatedAtAction("CreateBook", new { Id = bookDTo.Id }, book);
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDTO bookDTO)
        {
            var book = await _context.Books.FindAsync(id);
            if(book == null)
            {
                return NotFound();
            }
            var newBook = _mapper.Map(book, bookDTO);
            _context.Entry(newBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(! await BookExistAsync(id))
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

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if(book == null)
            {
                return NotFound();
            }
             _context.Books.Remove(book);
           await _context.SaveChangesAsync();

            return NoContent();
        }


        private async Task<bool> BookExistAsync(int id)
        {
            return await _context.Books.AnyAsync(c => c.Id == id);
        }



    }
    }
    

