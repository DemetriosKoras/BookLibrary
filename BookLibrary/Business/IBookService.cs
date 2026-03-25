using BookLibrary.Models;

namespace BookLibrary.Business;

public interface IBookService
{
    public IEnumerable<Book> AddBook(Book book, IEnumerable<Book> library);

    public IEnumerable<Book> SortBooks(IEnumerable<Book> library);

    public IEnumerable<Book> SearchBooks(IEnumerable<Book> library, string titlePart);

    public IEnumerable<Book> LoadBooks(string filePath);

    public void SaveBooks(IEnumerable<Book> library, string filePath);

}
