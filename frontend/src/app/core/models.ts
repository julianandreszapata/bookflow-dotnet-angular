export interface Book {
  id: string;
  title: string;
  author: string;
  isbn: string;
  totalCopies: number;
  availableCopies: number;
  createdAtUtc: string;
}

export interface Loan {
  id: string;
  bookId: string;
  bookTitle: string;
  borrowerName: string;
  loanedAtUtc: string;
  dueDateUtc: string;
  returnedAtUtc: string | null;
  isReturned: boolean;
}

export interface CreateBookRequest {
  title: string;
  author: string;
  isbn: string;
  totalCopies: number;
}

export interface CreateLoanRequest {
  bookId: string;
  borrowerName: string;
  dueDateUtc: string;
}
