using BookFlow.Domain.Entities;

namespace BookFlow.Application.Contracts;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Book?> GetByIsbnAsync(string isbn, CancellationToken cancellationToken);
    Task AddAsync(Book book, CancellationToken cancellationToken);
}

public interface ILoanRepository
{
    Task<IReadOnlyList<Loan>> GetAllWithBookAsync(CancellationToken cancellationToken);
    Task<Loan?> GetByIdWithBookAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Loan loan, CancellationToken cancellationToken);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
