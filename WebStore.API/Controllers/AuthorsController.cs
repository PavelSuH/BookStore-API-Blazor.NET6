#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.API.Data;
using WebStore.API.Models.Author;
using WebStore.API.Static;

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDTO>>> GetAuthors()
        {
            try
            {
                _logger.LogInformation("Process is starting");
                var authors = await _context.Authors.ToListAsync();
                var authosDTO = _mapper.Map<IEnumerable<AuthorReadOnlyDTO>>(authors);
                return Ok(authosDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Error performing get in {nameof(GetAuthors)}");
                return StatusCode(500, Messeges.Error500);
            }
           
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDTO>> GetAuthor(int id)
        {

            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    _logger.LogError($"Record hasn't been found: {nameof(GetAuthor)}");
                    return NotFound();
                }
                var authorsDTO = _mapper.Map<AuthorReadOnlyDTO>(author);

                return authorsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error performing get in {nameof(GetAuthors)}");
                return StatusCode(500, Messeges.Error500);
            }

           
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDTO authorDTO)
        {
           
            if (id != authorDTO.Id)
            {
                _logger.LogError("Bad request");
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);

            if(author == null)
            {
                _logger.LogInformation("Author not found");
                return NotFound();
            }
            _mapper.Map(authorDTO,author);


            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(AuthorCreateDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
