using BookLibrary.Business;
using BookLibrary.Models;
using BookLibrary.DAL;
using NSubstitute;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Test;

[Trait("Category", "Unit")]
public class BookServiceTests
{
    [Fact]
    public void AddBook_ShouldReturnCollectionWithNewBook()
    {
        // Arrange
        var bookService = new BookService();
        var booksCollection = new List<Book>
        {
            new() { Title = "Book 1", Author = "Author A", NumberOfPages = 100 },
            new() { Title = "Book 2", Author = "Author B", NumberOfPages = 200 }
        };
        var newBook = new Book { Title = "Book 3", Author = "Author C", NumberOfPages = 150 };

        // Act
        var updatedCollection = bookService.AddBook(newBook, booksCollection);

        // Assert
        Assert.Equal(newBook, updatedCollection.Last());
        Assert.Equal(3, updatedCollection.Count());
    }

    [Fact]
    public void SearchBooks_ShouldReturnBooksWhoseTitleContainsSearchPart()
    {
        // Arrange
        var bookService = new BookService();
        var booksCollection = new List<Book>
        {
            new() { Title = "Book One", Author = "Author A", NumberOfPages = 100 },
            new() { Title = "Book Two", Author = "Author B", NumberOfPages = 200 }
        };
        var searchPart = "Two";

        // Act
        var searchResults = bookService.SearchBooks(booksCollection, searchPart);

        // Assert
        Assert.Single(searchResults);
        Assert.Contains(booksCollection[1], searchResults);
    }

    [Fact]
    public void SortBooks_ShouldSortByAuthorThenByTitle()
    {
        // Arrange
        var bookService = new BookService();
        var booksCollection = new List<Book>
        {
            new() { Title = "Book One", Author = "Author A", NumberOfPages = 100 },
            new() { Title = "Book Two", Author = "Author B", NumberOfPages = 100 },
            new() { Title = "AAA", Author = "Author B", NumberOfPages = 200 },
            new() { Title = "AAA", Author = "Author A", NumberOfPages = 200 },
        };

        // Act
        var sortedBooks = bookService.SortBooks(booksCollection).ToList();

        // Assert
        Assert.Equal(booksCollection[3], sortedBooks[0]);
        Assert.Equal(booksCollection[0], sortedBooks[1]);
        Assert.Equal(booksCollection[2], sortedBooks[2]);
        Assert.Equal(booksCollection[1], sortedBooks[3]);
    }

    [Fact]
    public void LoadBooks_ShouldReturnBooksFromRepository()
    {
        // Arrange
        var filePath = "books.xml";
        var expectedBooks = new List<Book>
        {
            new() { Title = "Book One", Author = "Author A", NumberOfPages = 100 },
            new() { Title = "Book Two", Author = "Author B", NumberOfPages = 200 }
        };

        var xmlRepositoryMock = Substitute.For<IBookRepository>();
        xmlRepositoryMock.LoadBooks(filePath).Returns(expectedBooks);

        var bookService = new BookService(xmlRepositoryMock);

        // Act
        var loadedBooks = bookService.LoadBooks(filePath);

        // Assert
        xmlRepositoryMock.Received(1).LoadBooks(filePath);
        Assert.Equal(expectedBooks, loadedBooks);
    }

    [Fact]
    public void SaveBooks_ShouldCallRepositorySaveBooks()
    {
        // Arrange
        var filePath = "books.xml";
        var booksToSave = new List<Book>
        {
            new() { Title = "Book One", Author = "Author A", NumberOfPages = 100 },
            new() { Title = "Book Two", Author = "Author B", NumberOfPages = 200 }
        };
        var xmlRepositoryMock = Substitute.For<IBookRepository>();
        var bookService = new BookService(xmlRepositoryMock);

        // Act
        bookService.SaveBooks(booksToSave, filePath);

        // Assert
        xmlRepositoryMock.Received(1).SaveBooks(booksToSave, filePath);
    }

    [Fact]
    public void AddBook_ShouldFail_WhenBookIsInvalid()
    {
        // Arrange
        var bookService = new BookService();
        var booksCollection = new List<Book>();
        var invalidBook = new Book { Title = "", Author = "Author", NumberOfPages = 100 };

        // Act & Assert
        Assert.Throws<ValidationException>(() => bookService.AddBook(invalidBook, booksCollection));
        Assert.NotEqual(booksCollection, booksCollection.Append(invalidBook).ToList());
    }

    [Fact]
    public void LoadBooks_ShouldFail_WhenRepositoryReturnsInvalidBook()
    {
        // Arrange
        var filePath = "books.xml";
        var invalidBooks = new List<Book>
        {
            new() { Title = "", Author = "Author", NumberOfPages = 100 }
        };
        var xmlRepositoryMock = Substitute.For<IBookRepository>();
        xmlRepositoryMock.LoadBooks(filePath).Returns(invalidBooks);
        var bookService = new BookService(xmlRepositoryMock);

        // Act & Assert
        Assert.Throws<ValidationException>(() => bookService.LoadBooks(filePath));
        xmlRepositoryMock.Received(1).LoadBooks(filePath);
    }

    [Fact]
    public void SaveBooks_ShouldThrow_WhenBooksAreInvalid()
    {
        // Arrange
        var filePath = "books.xml";
        var invalidBooks = new List<Book>
        {
            new() { Title = "Book", Author = "Author", NumberOfPages = 0 }
        };
        var xmlRepositoryMock = Substitute.For<IBookRepository>();
        var bookService = new BookService(xmlRepositoryMock);

        // Act & Assert
        Assert.Throws<ValidationException>(() => bookService.SaveBooks(invalidBooks, filePath));
        xmlRepositoryMock.Received(0).SaveBooks(invalidBooks, filePath);
    }

}
