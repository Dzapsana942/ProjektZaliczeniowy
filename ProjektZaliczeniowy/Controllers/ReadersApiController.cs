using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.Data;
using ProjektZaliczeniowy.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadersApiController : ControllerBase
    {
        private readonly LibraryContext _context;

        public ReadersApiController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/ReadersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reader>>> GetReaders()
        {
            return await _context.Readers.ToListAsync();
        }

        // GET: api/ReadersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reader>> GetReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null) return NotFound();

            return reader;
        }

        // POST: api/ReadersApi
        [HttpPost]
        public async Task<ActionResult<Reader>> PostReader(Reader reader)
        {
            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReader), new { id = reader.Id }, reader);
        }

        // PUT: api/ReadersApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReader(int id, Reader reader)
        {
            if (id != reader.Id) return BadRequest();

            _context.Entry(reader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Readers.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/ReadersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null) return NotFound();

            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
