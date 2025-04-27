import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent {
  constructor(private authService: AuthService) { }

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  login() {
    this.authService.login();
  }
}
