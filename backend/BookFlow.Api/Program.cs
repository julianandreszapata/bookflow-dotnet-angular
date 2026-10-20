using BookFlow.Api.Infrastructure;
using BookFlow.Application.Contracts;
using BookFlow.Application.Services;
using BookFlow.Domain.Entities;
using BookFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy("AngularDev", policy =>
    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddDbContext<BookFlowDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BookFlow") ?? "Data Source=bookflow.db"));
builder.Services.AddScoped<IBookRepository, EfBookRepository>();
builder.Services.AddScoped<ILoanRepository, EfLoanRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<LoanService>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AngularDev");
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookFlowDbContext>();
    db.Database.EnsureCreated();
    if (!db.Books.Any())
    {
        db.Books.AddRange(
            new Book("Clean Code", "Robert C. Martin", "9780132350884", 3),
            new Book("The Pragmatic Programmer", "David Thomas and Andrew Hunt", "9780135957059", 2),
            new Book("Domain-Driven Design", "Eric Evans", "9780321125217", 1));
        db.SaveChanges();
    }
}

app.Run();
