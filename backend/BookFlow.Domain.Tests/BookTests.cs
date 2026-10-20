using BookFlow.Domain.Entities;
using Xunit;

namespace BookFlow.Domain.Tests;

public sealed class BookTests
{
    [Fact]
    public void Checkout_DecreasesAvailableCopies()
    {
        var book = new Book("Clean Code", "Robert C. Martin", "9780132350884", 2);
        book.CheckoutCopy();
        Assert.Equal(1, book.AvailableCopies);
    }

    [Fact]
    public void Checkout_WhenOutOfStock_ThrowsBusinessRuleException()
    {
        var book = new Book("Clean Code", "Robert C. Martin", "9780132350884", 1);
        book.CheckoutCopy();
        Assert.Throws<BusinessRuleException>(() => book.CheckoutCopy());
    }

    [Fact]
    public void ReturningCopies_CannotExceedInventory()
    {
        var book = new Book("Clean Code", "Robert C. Martin", "9780132350884", 1);
        Assert.Throws<BusinessRuleException>(() => book.ReturnCopy());
    }
}
