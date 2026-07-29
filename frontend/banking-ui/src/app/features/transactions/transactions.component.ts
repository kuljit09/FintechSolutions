import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { TransactionService } from '../../core/services/transaction.service';
import { Transaction } from '../../core/models/transaction.model';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [DatePipe],
  template: `
    <div style="padding:20px; max-width:800px;">
      <h2>Transactions</h2>

      @for (t of transactions(); track t.id) {
        <div class="card" style="margin-bottom:10px;">
          <div style="display:flex; justify-content:space-between;">
            <b>{{ t.merchant || t.description || t.type }}</b>
            <span [style.color]="t.status === 'Failed' ? '#B42318' : '#1f2430'">{{ t.currency }} {{ t.amount }}</span>
          </div>
          <p style="color:#6B7280; font-size:12px;">{{ t.timestamp | date:'medium' }} · {{ t.status }}</p>

          @if (t.status === 'Failed') {
            <button (click)="explainFailure(t.id)">Why did this fail?</button>
          }
          @if (t.status === 'Completed') {
            <button (click)="checkDispute(t.id)">Can I dispute this?</button>
            @if (eligibleForDispute()[t.id]) {
              <button (click)="fileDispute(t.id)">File dispute</button>
            }
          }

          @if (explanations()[t.id]) {
            <p style="margin-top:8px; padding:8px; background:#FEF0C7; border-radius:6px; font-size:13px;">{{ explanations()[t.id] }}</p>
          }
          @if (disputeNotes()[t.id]) {
            <p style="margin-top:8px; padding:8px; background:#E7F4EE; border-radius:6px; font-size:13px;">{{ disputeNotes()[t.id] }}</p>
          }
        </div>
      }
    </div>
  `
})
export class TransactionsComponent {
  private route = inject(ActivatedRoute);
  private transactionService = inject(TransactionService);

  transactions = signal<Transaction[]>([]);
  explanations = signal<Record<string, string>>({});
  disputeNotes = signal<Record<string, string>>({});
  eligibleForDispute = signal<Record<string, boolean>>({});

  constructor() {
    const accountId = this.route.snapshot.paramMap.get('id')!;
    this.transactionService.getForAccount(accountId).subscribe(t => this.transactions.set(t));
  }

  explainFailure(transactionId: string) {
    this.transactionService.explainFailure(transactionId).subscribe(res => {
      this.explanations.update(m => ({ ...m, [transactionId]: res.explanation }));
    });
  }

  checkDispute(transactionId: string) {
    this.transactionService.checkDisputeEligibility(transactionId).subscribe(res => {
      this.disputeNotes.update(m => ({
        ...m,
        [transactionId]: res.eligible ? `Eligible to dispute: ${res.reason}` : `Not eligible: ${res.reason}`
      }));
      this.eligibleForDispute.update(m => ({ ...m, [transactionId]: res.eligible }));
    });
  }

  fileDispute(transactionId: string) {
    this.transactionService.fileDispute(transactionId, 'Customer does not recognize this charge').subscribe(() => {
      this.disputeNotes.update(m => ({ ...m, [transactionId]: 'Dispute filed - it is now under investigation.' }));
      this.eligibleForDispute.update(m => ({ ...m, [transactionId]: false }));
    });
  }
}
