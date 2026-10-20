namespace BookFlow.Domain.Entities;

public sealed class Loan
{
    private Loan() { } // EF Core materialization

    private Loan(Guid bookId, string borrowerName, DateTime dueDateUtc)
    {
        Id = Guid.NewGuid();
        BookId = bookId;
        BorrowerName = borrowerName.Trim();
        LoanedAtUtc = DateTime.UtcNow;
        DueDateUtc = dueDateUtc.Kind == DateTimeKind.Utc ? dueDateUtc : dueDateUtc.ToUniversalTime();
    }

    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public Book? Book { get; private set; }
    public string BorrowerName { get; private set; } = string.Empty;
    public DateTime LoanedAtUtc { get; private set; }
    public DateTime DueDateUtc { get; private set; }
    public DateTime? ReturnedAtUtc { get; private set; }
    public bool IsReturned => ReturnedAtUtc.HasValue;

    public static Loan Create(Guid bookId, string borrowerName, DateTime dueDateUtc)
    {
        if (bookId == Guid.Empty) throw new BusinessRuleException("A valid book is required.");
        if (string.IsNullOrWhiteSpace(borrowerName)) throw new BusinessRuleException("Borrower name is required.");
        if (dueDateUtc <= DateTime.UtcNow) throw new BusinessRuleException("Due date must be in the future.");
        return new Loan(bookId, borrowerName, dueDateUtc);
    }

    public void MarkReturned()
    {
        if (IsReturned) throw new BusinessRuleException("This loan has already been returned.");
        ReturnedAtUtc = DateTime.UtcNow;
    }
}
