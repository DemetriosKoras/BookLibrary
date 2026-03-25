using BookLibrary.DAL;
using BookLibrary.Models;
using FluentAssertions;

namespace BookLibrary.Test;

[Trait("Category", "Integration")]
public class XmlBookRepositoryTests
{
    [Fact]
    public void LoadBooks_ShouldLoadBooksFromXmlFile()
    {
        // Arrange
        var repository = new XmlBookRepository();
        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml");
        var xml = """
                  <?xml version="1.0" encoding="utf-8"?>
                  <ArrayOfBook xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                               xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                    <Book>
                      <Title>Book One</Title>
                      <Author>Author A</Author>
                      <NumberOfPages>100</NumberOfPages>
                    </Book>
                    <Book>
                      <Title>Book Two</Title>
                      <Author>Author B</Author>
                      <NumberOfPages>200</NumberOfPages>
                    </Book>
                  </ArrayOfBook>
                  """;
        try
        {
            File.WriteAllText(filePath, xml);

            // Act
            var result = repository.LoadBooks(filePath).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("Book One");
            result[0].Author.Should().Be("Author A");
            result[0].NumberOfPages.Should().Be(100);
            result[1].Title.Should().Be("Book Two");
            result[1].Author.Should().Be("Author B");
            result[1].NumberOfPages.Should().Be(200);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void SaveBooks_ShouldSaveBooksToXmlFile()
    {
        // Arrange
        var repository = new XmlBookRepository();
        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml");
        var books = new List<Book>
        {
            new() { Title = "Book One", Author = "Author A", NumberOfPages = 100 },
            new() { Title = "Book Two", Author = "Author B", NumberOfPages = 200 }
        };
        try
        {
            // Act
            repository.SaveBooks(books, filePath);

            // Assert
            File.Exists(filePath).Should().BeTrue();

            var loadedBooks = repository.LoadBooks(filePath).ToList();

            loadedBooks.Should().HaveCount(2);
            loadedBooks[0].Title.Should().Be("Book One");
            loadedBooks[0].Author.Should().Be("Author A");
            loadedBooks[0].NumberOfPages.Should().Be(100);
            loadedBooks[1].Title.Should().Be("Book Two");
            loadedBooks[1].Author.Should().Be("Author B");
            loadedBooks[1].NumberOfPages.Should().Be(200);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void LoadBooks_ShouldThrowException_WhenXmlIsInvaid()
    {
        // Arrange
        var repository = new XmlBookRepository();
        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml");
        var xml = "Haha im not even close to xml!!!asdadasd";
        try
        { 
            File.WriteAllText(filePath, xml);

            // Act 
            var act = () => repository.LoadBooks(filePath);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
