using BookLibrary.Models;

namespace BookLibrary.DAL;

internal interface IBookRepository
{
    /// <summary>
    /// Loads a collection of books from the specified file.
    /// </summary>
    /// <param name="filePath">The path to the file containing book data. The file must exist and be accessible.</param>
    /// <returns>An enumerable collection of books loaded from the file. The collection is empty if the file contains no books.</returns>
    public IEnumerable<Book> LoadBooks(string filePath);

    /// <summary>
    /// Saves the specified collection of books to the file at the given path.
    /// </summary>
    /// <param name="books">The collection of books to save. Cannot be null.</param>
    /// <param name="filePath">The path of the file to which the books will be saved. Cannot be null or empty.</param>
    public void SaveBooks(IEnumerable<Book> books, string filePath);
}
