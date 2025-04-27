import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MainNavbarComponent } from "./main-navbar/main-navbar.component";
import { AuthService } from './auth.service';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MainNavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(authService: AuthService) {
    authService.configure();
  }
}
