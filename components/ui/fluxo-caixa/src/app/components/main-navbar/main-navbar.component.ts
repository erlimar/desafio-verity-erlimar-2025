import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AppUserInfo } from '../../models/app-user-info.model';
import { CommonModule } from '@angular/common';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-main-navbar',
  imports: [RouterLink, CommonModule],
  templateUrl: './main-navbar.component.html',
  styleUrl: './main-navbar.component.css'
})
export class MainNavbarComponent {
  private authService = inject(AuthService);

  get userInfo(): AppUserInfo | null {
    return this.authService.getUserInfo() || null;
  }

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  get manageAccountUrl(): string {
    return environment.oidc.manageAccountUrl;
  }

  logout() {
    this.authService.logout();
  }
}
