import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Card } from '../models/card.model';
import { API_BASE_URL } from './api-config';

@Injectable({ providedIn: 'root' })
export class CardService {
  private http = inject(HttpClient);

  getById(cardId: string) {
    return this.http.get<Card>(`${API_BASE_URL}/cards/${cardId}`);
  }

  // Direct REST path - the Angular confirm dialog IS the confirmation here (see CardsController comment).
  block(cardId: string, reason: string) {
    return this.http.post(`${API_BASE_URL}/cards/${cardId}/block`, { reason });
  }
}
