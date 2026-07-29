import { Routes } from '@angular/router';
import { AccountsComponent } from './features/accounts/accounts.component';
import { TransactionsComponent } from './features/transactions/transactions.component';
import { LoansComponent } from './features/loans/loans.component';
import { CardsComponent } from './features/cards/cards.component';
import { ChatComponent } from './features/chat/chat.component';

export const routes: Routes = [
  { path: '', redirectTo: 'accounts', pathMatch: 'full' },
  { path: 'accounts', component: AccountsComponent },
  { path: 'accounts/:id/transactions', component: TransactionsComponent },
  { path: 'loans', component: LoansComponent },
  { path: 'cards', component: CardsComponent },
  { path: 'chat', component: ChatComponent }
];
