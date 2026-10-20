import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BookFlowApiService } from './core/bookflow-api.service';
import { Book, Loan } from './core/models';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  private readonly api = inject(BookFlowApiService);
  books: Book[] = [];
  loans: Loan[] = [];
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  bookForm = { title: '', author: '', isbn: '', totalCopies: 1 };
  loanForm = { bookId: '', borrowerName: '', dueDate: '' };

  ngOnInit(): void { this.refresh(); }

  refresh(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.api.getBooks().subscribe({
      next: (items) => { this.books = items; this.loadLoans(); },
      error: () => { this.isLoading = false; this.errorMessage = 'Could not reach the API. Start the .NET backend on http://localhost:5080.'; }
    });
  }

  private loadLoans(): void {
    this.api.getLoans().subscribe({
      next: (items) => { this.loans = items; this.isLoading = false; },
      error: () => { this.isLoading = false; this.errorMessage = 'Books loaded, but loans could not be retrieved.'; }
    });
  }

  createBook(): void {
    this.clearMessages();
    this.api.createBook({ ...this.bookForm }).subscribe({
      next: () => { this.successMessage = 'Book registered.'; this.bookForm = { title: '', author: '', isbn: '', totalCopies: 1 }; this.refresh(); },
      error: (error: { error?: { error?: string; title?: string } }) => this.errorMessage = error.error?.error ?? error.error?.title ?? 'Book could not be registered.'
    });
  }

  createLoan(): void {
    this.clearMessages();
    if (!this.loanForm.dueDate) { this.errorMessage = 'Select a due date.'; return; }
    this.api.createLoan({ bookId: this.loanForm.bookId, borrowerName: this.loanForm.borrowerName,
      dueDateUtc: new Date(this.loanForm.dueDate).toISOString() }).subscribe({
      next: () => { this.successMessage = 'Loan registered.'; this.loanForm = { bookId: '', borrowerName: '', dueDate: '' }; this.refresh(); },
      error: (error: { error?: { error?: string; title?: string } }) => this.errorMessage = error.error?.error ?? error.error?.title ?? 'Loan could not be registered.'
    });
  }

  returnLoan(loan: Loan): void {
    this.clearMessages();
    this.api.returnLoan(loan.id).subscribe({
      next: () => { this.successMessage = 'Book returned.'; this.refresh(); },
      error: (error: { error?: { error?: string; title?: string } }) => this.errorMessage = error.error?.error ?? error.error?.title ?? 'Loan could not be returned.'
    });
  }

  availableBooks(): Book[] { return this.books.filter((book) => book.availableCopies > 0); }
  private clearMessages(): void { this.errorMessage = ''; this.successMessage = ''; }
}
