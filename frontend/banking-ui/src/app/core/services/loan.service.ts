import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Loan, LoanApplicationRequest } from '../models/loan.model';
import { API_BASE_URL } from './api-config';

@Injectable({ providedIn: 'root' })
export class LoanService {
  private http = inject(HttpClient);

  getById(loanId: string, customerId: string) {
    return this.http.get<Loan>(`${API_BASE_URL}/loans/${loanId}?customerId=${customerId}`);
  }

  apply(request: LoanApplicationRequest) {
    return this.http.post<Loan>(`${API_BASE_URL}/loans/apply`, request);
  }
}
