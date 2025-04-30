import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent {
  private authService = inject(AuthService);

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  login() {
    this.authService.login();
  }
}
