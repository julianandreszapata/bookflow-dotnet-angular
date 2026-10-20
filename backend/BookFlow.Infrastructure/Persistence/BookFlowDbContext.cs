using BookFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookFlow.Infrastructure.Persistence;

public sealed class BookFlowDbContext(DbContextOptions<BookFlowDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Author).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Isbn).HasMaxLength(32).IsRequired();
            entity.HasIndex(x => x.Isbn).IsUnique();
        });
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.BorrowerName).HasMaxLength(160).IsRequired();
            entity.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
