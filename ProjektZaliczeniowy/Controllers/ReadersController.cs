using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.Data;
using ProjektZaliczeniowy.Models;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Controllers
{
    [Authorize] // 🔐 Domyślnie tylko zalogowani użytkownicy mają dostęp do kontrolera
    public class ReadersController : Controller
    {
        private readonly LibraryContext _context;

        public ReadersController(LibraryContext context)
        {
            _context = context;
        }

        // 🔓 Każdy użytkownik (nawet niezalogowany) może przeglądać listę czytelników
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Readers.ToListAsync());
        }

        // 🔐 Tylko administrator może dodawać nowych czytelników
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber")] Reader reader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reader);
        }

        // 🔐 Edytowanie dostępne tylko dla administratora
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null) return NotFound();
            return View(reader);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber")] Reader reader)
        {
            if (id != reader.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(reader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reader);
        }

        // 🔐 Usuwanie dostępne tylko dla administratora
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var reader = await _context.Readers.FirstOrDefaultAsync(m => m.Id == id);
            if (reader == null) return NotFound();
            return View(reader);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader != null)
            {
                _context.Readers.Remove(reader);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
