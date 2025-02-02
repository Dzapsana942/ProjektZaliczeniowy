using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.Data;
using ProjektZaliczeniowy.Models;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Controllers
{
    [Authorize] // 🔐 Domyślnie tylko zalogowani użytkownicy mają dostęp do kontrolera
    public class LoansController : Controller
    {
        private readonly LibraryContext _context;

        public LoansController(LibraryContext context)
        {
            _context = context;
        }

        // 🔓 Każdy użytkownik (nawet niezalogowany) może przeglądać wypożyczenia
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var loans = _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader);
            return View(await loans.ToListAsync());
        }

        // 🔐 Tylko administrator może dodawać nowe wypożyczenia
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "LastName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,ReaderId,LoanDate,ReturnDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "LastName", loan.ReaderId);
            return View(loan);
        }

        // 🔐 Edytowanie dostępne tylko dla administratora
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null) return NotFound();

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "LastName", loan.ReaderId);
            return View(loan);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,ReaderId,LoanDate,ReturnDate")] Loan loan)
        {
            if (id != loan.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "LastName", loan.ReaderId);
            return View(loan);
        }

        // 🔐 Usuwanie dostępne tylko dla administratora
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null) return NotFound();
            return View(loan);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
