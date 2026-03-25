using BookLibrary.DAL;
using BookLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Business;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(): this(new XmlBookRepository())
    {

    }
    internal BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Book> AddBook(Book book, IEnumerable<Book> books)
    {
        ArgumentNullException.ThrowIfNull(book, nameof(book));
        ArgumentNullException.ThrowIfNull(books, nameof(books));

        ValidateBook(book);

        return books.Append(book).ToList();
    }

    public IEnumerable<Book> SearchBooks(IEnumerable<Book> books, string titlePart)
    {
        ArgumentNullException.ThrowIfNull(books, nameof(books));
        ArgumentException.ThrowIfNullOrWhiteSpace(titlePart, nameof(titlePart));

        return books.Where(b => b.Title.Contains(titlePart)).ToList();
    }

    public IEnumerable<Book> SortBooks(IEnumerable<Book> books)
    {
        ArgumentNullException.ThrowIfNull(books, nameof(books));

        return books.OrderBy(b => b.Author).ThenBy(b => b.Title).ToList();
    }

    public IEnumerable<Book> LoadBooks(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var books = _repository.LoadBooks(filePath);

        foreach (var book in books)
        {
            ValidateBook(book);
        }

        return books;
    }

    public void SaveBooks(IEnumerable<Book> books, string filePath)
    {
        ArgumentNullException.ThrowIfNull(books, nameof(books));
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));

        foreach (var book in books)
        {
            ValidateBook(book);
        }

        _repository.SaveBooks(books, filePath);
    }

    private static void ValidateBook(Book book)
    {
        var context = new ValidationContext(book);
        Validator.ValidateObject(book, context, validateAllProperties: true);
    }
}
