import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';
import { AppUserInfo } from '../app-user-info.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-main-navbar',
  imports: [RouterLink, CommonModule],
  templateUrl: './main-navbar.component.html',
  styleUrl: './main-navbar.component.css'
})
export class MainNavbarComponent {
  constructor(private authService: AuthService) { }

  get userInfo(): AppUserInfo | null {
    return this.authService.getUserInfo() || null;
  }

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  logout() {
    this.authService.logout();
  }
}
