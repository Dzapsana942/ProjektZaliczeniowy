using System;
using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowy.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Identyfikator książki jest wymagany.")]
        public int BookId { get; set; }

        [Required]
        public Book Book { get; set; } = null!;

        [Required(ErrorMessage = "Identyfikator czytelnika jest wymagany.")]
        public int ReaderId { get; set; }

        [Required]
        public Reader Reader { get; set; } = null!;

        [Required(ErrorMessage = "Data wypożyczenia jest wymagana.")]
        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }
    }
}
