namespace BookFlow.Application.Contracts;

public sealed record CreateBookRequest(string Title, string Author, string Isbn, int TotalCopies);
public sealed record BookResponse(Guid Id, string Title, string Author, string Isbn, int TotalCopies, int AvailableCopies, DateTime CreatedAtUtc);
public sealed record CreateLoanRequest(Guid BookId, string BorrowerName, DateTime DueDateUtc);
public sealed record LoanResponse(Guid Id, Guid BookId, string BookTitle, string BorrowerName, DateTime LoanedAtUtc, DateTime DueDateUtc, DateTime? ReturnedAtUtc, bool IsReturned);
