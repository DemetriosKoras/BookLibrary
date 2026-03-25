using BookLibrary.Models;
using System.Xml.Serialization;

namespace BookLibrary.DAL;

internal sealed class XmlBookRepository() : IBookRepository
{
    private readonly XmlSerializer _serializer = new(typeof(List<Book>));

    public IEnumerable<Book> LoadBooks(string filePath)
    {
        if (!File.Exists(filePath)) return [];

        using var stream = File.OpenRead(filePath);
        var books = (List<Book>)_serializer.Deserialize(stream)!;

        return books ?? [];
    }

    public void SaveBooks(IEnumerable<Book> books, string filePath)
    {
        using var stream = File.Create(filePath);
        _serializer.Serialize(stream, books.ToList());
    }
}
