using NUnit.Framework;
using ProjektZaliczeniowy.Data;
using ProjektZaliczeniowy.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Tests
{
    public class LibraryServiceTests
    {
        private LibraryContext _context;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "TestLibraryDB")
                .Options;
            _context = new LibraryContext(options);

            _context.Books.Add(new Book { Title = "Testowa Książka", Author = "Autor Testowy", ISBN = "123456789", Year = 2020, IsAvailable = true });
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Test]
        public async Task GetBooks_ReturnsCorrectNumberOfBooks()
        {
            var books = await _context.Books.ToListAsync();
            Assert.AreEqual(1, books.Count);
        }

        [Test]
        public async Task AddBook_IncreasesBookCount()
        {
            _context.Books.Add(new Book { Title = "Druga Książka", Author = "Autor Drugi", ISBN = "987654321", Year = 2021, IsAvailable = true });
            await _context.SaveChangesAsync();

            var books = await _context.Books.ToListAsync();
            Assert.AreEqual(2, books.Count);
        }

        [Test]
        public async Task DeleteBook_DecreasesBookCount()
        {
            var book = await _context.Books.FirstAsync();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            var books = await _context.Books.ToListAsync();
            Assert.AreEqual(0, books.Count);
        }
    }
}
