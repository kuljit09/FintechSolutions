import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ChatRequest, ChatResponse } from '../models/chat.model';
import { API_BASE_URL } from './api-config';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private http = inject(HttpClient);

  send(request: ChatRequest) {
    return this.http.post<ChatResponse>(`${API_BASE_URL}/chat`, request);
  }
}
