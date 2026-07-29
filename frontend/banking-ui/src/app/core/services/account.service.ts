import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Account } from '../models/account.model';
import { API_BASE_URL } from './api-config';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private http = inject(HttpClient);

  getForCustomer(customerId: string) {
    return this.http.get<Account[]>(`${API_BASE_URL}/customers/${customerId}/accounts`);
  }

  getById(accountId: string, customerId: string) {
    return this.http.get<Account>(`${API_BASE_URL}/accounts/${accountId}?customerId=${customerId}`);
  }
}
