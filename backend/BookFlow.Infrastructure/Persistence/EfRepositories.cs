using BookFlow.Application.Contracts;
using BookFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookFlow.Infrastructure.Persistence;

public sealed class EfBookRepository(BookFlowDbContext db) : IBookRepository
{
    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct) =>
        await db.Books.AsNoTracking().OrderBy(x => x.Title).ToListAsync(ct);
    public Task<Book?> GetByIdAsync(Guid id, CancellationToken ct) => db.Books.FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task<Book?> GetByIsbnAsync(string isbn, CancellationToken ct) => db.Books.FirstOrDefaultAsync(x => x.Isbn == isbn, ct);
    public Task AddAsync(Book book, CancellationToken ct) => db.Books.AddAsync(book, ct).AsTask();
}

public sealed class EfLoanRepository(BookFlowDbContext db) : ILoanRepository
{
    public async Task<IReadOnlyList<Loan>> GetAllWithBookAsync(CancellationToken ct) =>
        await db.Loans.Include(x => x.Book).AsNoTracking().OrderByDescending(x => x.LoanedAtUtc).ToListAsync(ct);
    public Task<Loan?> GetByIdWithBookAsync(Guid id, CancellationToken ct) => db.Loans.Include(x => x.Book).FirstOrDefaultAsync(x => x.Id == id, ct);
    public Task AddAsync(Loan loan, CancellationToken ct) => db.Loans.AddAsync(loan, ct).AsTask();
}

public sealed class EfUnitOfWork(BookFlowDbContext db) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
