import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoanService } from '../../core/services/loan.service';
import { CURRENT_CUSTOMER_ID } from '../../core/services/api-config';
import { Loan } from '../../core/models/loan.model';

@Component({
  selector: 'app-loans',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div style="padding:20px; max-width:600px;">
      <h2>Apply for a Loan</h2>
      <p style="color:#6B7280; font-size:13px;">
        This demonstrates a synchronous, explainable decision via <code>LoanEligibilityPolicy</code> -
        approval/rejection happens immediately with a stated reason, not a black-box wait.
      </p>

      <div style="display:flex; flex-direction:column; gap:8px; max-width:360px;">
        <select [ngModel]="loanType()" (ngModelChange)="loanType.set($event)">
          <option value="Personal">Personal</option>
          <option value="Auto">Auto</option>
          <option value="Home">Home</option>
          <option value="Education">Education</option>
        </select>
        <input type="number" [ngModel]="principal()" (ngModelChange)="principal.set($event)" placeholder="Loan amount">
        <input type="number" [ngModel]="term()" (ngModelChange)="term.set($event)" placeholder="Term (months)">
        <input type="number" [ngModel]="income()" (ngModelChange)="income.set($event)" placeholder="Estimated annual income">
        <button (click)="apply()">Apply</button>
      </div>

      @if (result()) {
        <div class="card" style="margin-top:16px;" [style.borderColor]="result()!.status === 'Approved' ? '#0F7B4D' : '#B42318'">
          <b>{{ result()!.status }}</b>
          @if (result()!.status === 'Approved') {
            <p>Interest rate: {{ result()!.interestRatePercent }}% over {{ result()!.termMonths }} months</p>
          } @else {
            <p style="color:#B42318;">{{ result()!.rejectionReason }}</p>
          }
        </div>
      }
    </div>
  `
})
export class LoansComponent {
  private loanService = inject(LoanService);

  loanType = signal('Personal');
  principal = signal(100000);
  term = signal(24);
  income = signal(600000);
  result = signal<Loan | null>(null);

  apply() {
    this.loanService.apply({
      customerId: CURRENT_CUSTOMER_ID,
      loanType: this.loanType(),
      principalAmount: this.principal(),
      termMonths: this.term(),
      annualIncomeEstimate: this.income()
    }).subscribe(loan => this.result.set(loan));
  }
}
