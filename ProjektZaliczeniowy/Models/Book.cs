using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowy.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Autor jest wymagany.")]
        [Display(Name = "Autor")]
        public string Author { get; set; } = null!;

        [Required(ErrorMessage = "ISBN jest wymagany.")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = null!;

        [Range(1000, 2100, ErrorMessage = "Rok wydania musi być między 1000 a 2100.")]
        [Display(Name = "Rok wydania")]
        public int Year { get; set; }

        [Display(Name = "Dostępność")]
        public bool IsAvailable { get; set; } = true;
    }
}
