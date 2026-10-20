using BookFlow.Application.Contracts;
using BookFlow.Domain.Entities;

namespace BookFlow.Application.Services;

public sealed class LoanService(IBookRepository books, ILoanRepository loans, IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<LoanResponse>> GetAllAsync(CancellationToken ct) =>
        (await loans.GetAllWithBookAsync(ct)).Select(Map).ToList();

    public async Task<LoanResponse> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var loan = await loans.GetByIdWithBookAsync(id, ct) ?? throw new KeyNotFoundException("Loan was not found.");
        return Map(loan);
    }

    public async Task<LoanResponse> CreateAsync(CreateLoanRequest request, CancellationToken ct)
    {
        var book = await books.GetByIdAsync(request.BookId, ct) ?? throw new KeyNotFoundException("Book was not found.");
        book.CheckoutCopy();
        var loan = Loan.Create(request.BookId, request.BorrowerName, request.DueDateUtc);
        await loans.AddAsync(loan, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return new LoanResponse(loan.Id, book.Id, book.Title, loan.BorrowerName, loan.LoanedAtUtc,
            loan.DueDateUtc, loan.ReturnedAtUtc, loan.IsReturned);
    }

    public async Task<LoanResponse> ReturnAsync(Guid id, CancellationToken ct)
    {
        var loan = await loans.GetByIdWithBookAsync(id, ct) ?? throw new KeyNotFoundException("Loan was not found.");
        if (loan.Book is null) throw new InvalidOperationException("The loan's book relationship is missing.");
        loan.MarkReturned();
        loan.Book.ReturnCopy();
        await unitOfWork.SaveChangesAsync(ct);
        return Map(loan);
    }

    private static LoanResponse Map(Loan loan) => new(loan.Id, loan.BookId, loan.Book?.Title ?? "Unknown book",
        loan.BorrowerName, loan.LoanedAtUtc, loan.DueDateUtc, loan.ReturnedAtUtc, loan.IsReturned);
}
