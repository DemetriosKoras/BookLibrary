using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Models;

public class Book
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Author { get; set; }

    [Range(1, int.MaxValue)]
    public int NumberOfPages { get; set; }
}
