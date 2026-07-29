import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  template: `
    <nav style="padding:12px 20px; background:#0B2545; display:flex; gap:20px;">
      <a routerLink="/accounts" style="color:#CFE0FF; text-decoration:none;">Accounts</a>
      <a routerLink="/loans" style="color:#CFE0FF; text-decoration:none;">Loans</a>
      <a routerLink="/cards" style="color:#CFE0FF; text-decoration:none;">Cards</a>
      <a routerLink="/chat" style="color:#CFE0FF; text-decoration:none;">Support Chat</a>
    </nav>
    <router-outlet />
  `
})
export class AppComponent {}
