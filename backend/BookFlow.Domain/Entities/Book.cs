namespace BookFlow.Domain.Entities;

public sealed class Book
{
    private Book() { } // EF Core materialization

    public Book(string title, string author, string isbn, int totalCopies)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new BusinessRuleException("Title is required.");
        if (string.IsNullOrWhiteSpace(author)) throw new BusinessRuleException("Author is required.");
        if (string.IsNullOrWhiteSpace(isbn)) throw new BusinessRuleException("ISBN is required.");
        if (totalCopies < 1) throw new BusinessRuleException("Total copies must be at least 1.");

        Id = Guid.NewGuid();
        Title = title.Trim();
        Author = author.Trim();
        Isbn = isbn.Trim();
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string Isbn { get; private set; } = string.Empty;
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public void CheckoutCopy()
    {
        if (AvailableCopies <= 0) throw new BusinessRuleException("No copies are currently available.");
        AvailableCopies--;
    }

    public void ReturnCopy()
    {
        if (AvailableCopies >= TotalCopies) throw new BusinessRuleException("Returned copies cannot exceed the inventory.");
        AvailableCopies++;
    }
}
