import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../../core/services/chat.service';
import { CURRENT_CUSTOMER_ID } from '../../core/services/api-config';
import { ChatMessage } from '../../core/models/chat.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div style="padding:20px; max-width:680px; margin:0 auto;">
      <h2>Support Chat</h2>
      <p style="color:#6B7280; font-size:12px;">
        Try: "Why did my last payment fail?" · "Can I dispute the Flipkart charge?" ·
        "Block my debit card, I lost it" (then reply "yes" to actually confirm it).
      </p>

      <div class="card" style="height:440px; overflow-y:auto;">
        @for (m of messages(); track $index) {
          <div style="margin-bottom:14px;" [style.textAlign]="m.role === 'user' ? 'right' : 'left'">
            <div
              [style.background]="m.role === 'user' ? '#0B2545' : '#F4F6FB'"
              [style.color]="m.role === 'user' ? 'white' : '#1f2430'"
              style="display:inline-block; padding:10px 14px; border-radius:10px; max-width:85%; text-align:left;">
              {{ m.text }}
            </div>

            @if (m.requiresConfirmation) {
              <div style="margin-top:4px;">
                <span class="risk-badge high">Awaiting your confirmation - reply "yes" to proceed</span>
              </div>
            }

            @if (m.role === 'assistant' && (m.sourcesUsed?.length || m.toolsInvoked?.length)) {
              <details style="margin-top:4px; font-size:12px; color:#6B7280;">
                <summary>How I got this answer</summary>
                @if (m.toolsInvoked?.length) { <div><b>Tools invoked:</b> {{ m.toolsInvoked!.join(', ') }}</div> }
                @if (m.sourcesUsed?.length) { <div><b>Sources used:</b> {{ m.sourcesUsed!.join(' | ') }}</div> }
              </details>
            }
          </div>
        }
        @if (isLoading()) { <p style="color:#6B7280; font-style:italic;">Thinking...</p> }
      </div>

      <div style="display:flex; gap:8px; margin-top:12px;">
        <input
          [ngModel]="draft()"
          (ngModelChange)="draft.set($event)"
          (keydown.enter)="send()"
          placeholder="Ask about a transaction, dispute, loan, or card..."
          style="flex:1; padding:10px; border:1px solid #E2E8F0; border-radius:6px;">
        <button (click)="send()" [disabled]="isLoading() || !draft().trim()">Send</button>
      </div>
    </div>
  `
})
export class ChatComponent {
  private chatService = inject(ChatService);

  messages = signal<ChatMessage[]>([]);
  isLoading = signal(false);
  draft = signal('');
  private conversationId: string | undefined;

  send() {
    const text = this.draft().trim();
    if (!text) return;

    this.messages.update(m => [...m, { role: 'user', text }]);
    this.draft.set('');
    this.isLoading.set(true);

    this.chatService.send({
      customerId: CURRENT_CUSTOMER_ID,
      message: text,
      conversationId: this.conversationId
    }).subscribe({
      next: res => {
        this.conversationId = res.conversationId;
        this.messages.update(m => [...m, {
          role: 'assistant',
          text: res.answer,
          sourcesUsed: res.sourcesUsed,
          toolsInvoked: res.toolsInvoked,
          requiresConfirmation: res.requiresHumanConfirmation
        }]);
        this.isLoading.set(false);
      },
      error: () => {
        this.messages.update(m => [...m, { role: 'assistant', text: 'Sorry, something went wrong. Please try again.' }]);
        this.isLoading.set(false);
      }
    });
  }
}
