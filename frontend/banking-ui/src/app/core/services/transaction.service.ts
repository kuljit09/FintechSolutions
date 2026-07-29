import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DisputeEligibility, Transaction, TransactionFailureExplanation } from '../models/transaction.model';
import { API_BASE_URL } from './api-config';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  private http = inject(HttpClient);

  getForAccount(accountId: string) {
    return this.http.get<Transaction[]>(`${API_BASE_URL}/accounts/${accountId}/transactions`);
  }

  explainFailure(transactionId: string) {
    return this.http.get<TransactionFailureExplanation>(`${API_BASE_URL}/transactions/${transactionId}/explain-failure`);
  }

  checkDisputeEligibility(transactionId: string) {
    return this.http.get<DisputeEligibility>(`${API_BASE_URL}/transactions/${transactionId}/dispute-eligibility`);
  }

  fileDispute(transactionId: string, reason: string) {
    return this.http.post(`${API_BASE_URL}/transactions/${transactionId}/disputes`, { reason });
  }
}
