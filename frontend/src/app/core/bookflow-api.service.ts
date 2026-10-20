import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book, Loan, CreateBookRequest, CreateLoanRequest } from './models';

@Injectable({ providedIn: 'root' })
export class BookFlowApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5080/api';

  getBooks(): Observable<Book[]> { return this.http.get<Book[]>(`${this.baseUrl}/books`); }
  createBook(request: CreateBookRequest): Observable<Book> { return this.http.post<Book>(`${this.baseUrl}/books`, request); }
  getLoans(): Observable<Loan[]> { return this.http.get<Loan[]>(`${this.baseUrl}/loans`); }
  createLoan(request: CreateLoanRequest): Observable<Loan> { return this.http.post<Loan>(`${this.baseUrl}/loans`, request); }
  returnLoan(id: string): Observable<Loan> { return this.http.patch<Loan>(`${this.baseUrl}/loans/${id}/return`, {}); }
}
