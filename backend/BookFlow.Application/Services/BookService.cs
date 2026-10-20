using BookFlow.Application.Contracts;
using BookFlow.Domain.Entities;

namespace BookFlow.Application.Services;

public sealed class BookService(IBookRepository books, IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<BookResponse>> GetAllAsync(CancellationToken ct) =>
        (await books.GetAllAsync(ct)).Select(Map).ToList();

    public async Task<BookResponse> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var book = await books.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Book was not found.");
        return Map(book);
    }

    public async Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken ct)
    {
        var normalizedIsbn = request.Isbn?.Trim() ?? string.Empty;
        if (await books.GetByIsbnAsync(normalizedIsbn, ct) is not null)
            throw new BusinessRuleException("A book with this ISBN already exists.");

        var book = new Book(request.Title, request.Author, normalizedIsbn, request.TotalCopies);
        await books.AddAsync(book, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Map(book);
    }

    private static BookResponse Map(Book book) => new(book.Id, book.Title, book.Author, book.Isbn,
        book.TotalCopies, book.AvailableCopies, book.CreatedAtUtc);
}
