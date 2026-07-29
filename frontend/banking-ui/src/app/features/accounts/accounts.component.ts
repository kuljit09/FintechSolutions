import { Component, inject, resource } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { AccountService } from '../../core/services/account.service';
import { CURRENT_CUSTOMER_ID } from '../../core/services/api-config';

@Component({
  selector: 'app-accounts',
  standalone: true,
  imports: [RouterLink, DecimalPipe],
  template: `
    <div style="padding:20px; max-width:800px;">
      <h2>Your Accounts</h2>

      @if (accountsResource.isLoading()) {
        <p>Loading accounts...</p>
      } @else {
        <div style="display:grid; grid-template-columns:repeat(auto-fill,minmax(260px,1fr)); gap:16px;">
          @for (a of accountsResource.value(); track a.id) {
            <div class="card">
              <div style="display:flex; justify-content:space-between;">
                <b>{{ a.type }}</b>
                <span style="color:#6B7280; font-size:12px;">{{ a.status }}</span>
              </div>
              <p style="color:#6B7280; font-size:13px;">{{ a.accountNumber }}</p>
              <p style="font-size:22px; font-weight:600;">{{ a.currency }} {{ a.balance | number:'1.2-2' }}</p>
              @if (a.overdraftLimit > 0) {
                <p style="font-size:12px; color:#6B7280;">Overdraft limit: {{ a.currency }} {{ a.overdraftLimit }}</p>
              }
              <a [routerLink]="['/accounts', a.id, 'transactions']">View transactions →</a>
            </div>
          }
        </div>
      }
    </div>
  `
})
export class AccountsComponent {
  private accountService = inject(AccountService);

  accountsResource = resource({
    loader: () => firstValueFrom(this.accountService.getForCustomer(CURRENT_CUSTOMER_ID))
  });
}
