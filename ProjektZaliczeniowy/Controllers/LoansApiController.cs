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
    public class LoansApiController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoansApiController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null) return NotFound();
            return loan;
        }

        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan(Loan loan)
        {
            if (loan == null) return BadRequest("Dane wypożyczenia są puste.");

            var book = await _context.Books.FindAsync(loan.BookId);
            var reader = await _context.Readers.FindAsync(loan.ReaderId);

            if (book == null || reader == null)
            {
                return BadRequest("Książka lub czytelnik nie istnieją.");
            }

            bool isBookLoaned = await _context.Loans.AnyAsync(l => l.BookId == loan.BookId && l.ReturnDate == null);
            if (isBookLoaned)
            {
                return BadRequest("Ta książka jest już wypożyczona.");
            }

            loan.Book = book;
            loan.Reader = reader;
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(int id, Loan loan)
        {
            if (id != loan.Id) return BadRequest();

            var existingLoan = await _context.Loans.FindAsync(id);
            if (existingLoan == null) return NotFound();

            existingLoan.ReturnDate = loan.ReturnDate;
            _context.Entry(existingLoan).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return NotFound();

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
