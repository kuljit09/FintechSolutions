import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardService } from '../../core/services/card.service';

@Component({
  selector: 'app-cards',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div style="padding:20px; max-width:600px;">
      <h2>Card Lookup</h2>
      <p style="color:#6B7280; font-size:13px;">
        Blocking a card here uses Angular's own confirm dialog as the human-in-the-loop gate -
        the chatbot path (see Support Chat) uses a different, conversational confirmation flow
        for the exact same underlying <code>ICardService.BlockCardAsync</code> operation.
      </p>

      <div style="display:flex; gap:8px;">
        <input [ngModel]="cardId()" (ngModelChange)="cardId.set($event)" placeholder="Card id (GUID)" style="flex:1;">
        <button (click)="lookup()">Look up</button>
      </div>

      @if (card()) {
        <div class="card" style="margin-top:16px;">
          <p><b>{{ card()!.maskedNumber }}</b> <span class="risk-badge read">{{ card()!.type }}</span></p>
          <p>Status: {{ card()!.status }}</p>
          @if (card()!.status === 'Active') {
            <button (click)="requestBlock()" style="background:#B42318; color:white; border:none; padding:8px 14px; border-radius:6px;">
              Report lost / Block card
            </button>
          }
        </div>
      }

      @if (blockResult()) {
        <p style="margin-top:12px; padding:8px; background:#E7F4EE; border-radius:6px;">{{ blockResult() }}</p>
      }
    </div>
  `
})
export class CardsComponent {
  private cardService = inject(CardService);

  cardId = signal('');
  card = signal<any>(null);
  blockResult = signal<string | null>(null);

  lookup() {
    if (!this.cardId().trim()) return;
    this.cardService.getById(this.cardId()).subscribe(c => this.card.set(c));
  }

  requestBlock() {
    // The window.confirm() call IS the human-in-the-loop confirmation for this direct-UI path.
    const confirmed = window.confirm('Blocking this card cannot be undone from the app. Continue?');
    if (!confirmed) return;

    this.cardService.block(this.cardId(), 'Reported lost via web app').subscribe((res: any) => {
      this.blockResult.set(res.message);
      this.lookup();
    });
  }
}
