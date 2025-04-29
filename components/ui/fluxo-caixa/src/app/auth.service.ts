import { inject, Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { AppUserInfo } from './app-user-info.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private oAuthService = inject(OAuthService);
  private router = inject(Router);

  configure(): void {
    this.oAuthService.configure(authConfig);
    this.oAuthService.loadDiscoveryDocumentAndTryLogin().then(() => {
      if (this.oAuthService.hasValidAccessToken()) {
        const userNickname = this.oAuthService.getIdentityClaims()['preferred_username'];
        console.info(`O usuário está autenticado como "${userNickname}"!`, this.oAuthService.getIdentityClaims());
      } else {
        console.warn('O usuário não está autenticado.');
      }
    });
  }

  isAuthenticated(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  getUserInfo(): AppUserInfo | null {
    const claims = this.oAuthService.getIdentityClaims();
    if (claims) {
      return {
        name: claims['name'] || claims['nickname'] || claims['preferred_username'],
        nickname: claims['nickname'] || claims['preferred_username'],
      };
    }
    return null;
  }

  login(): void {
    this.oAuthService.configure(authConfig);

    this.oAuthService.loadDiscoveryDocumentAndLogin().then(() => {
      if (this.oAuthService.hasValidAccessToken()) {
        const userNickname = this.oAuthService.getIdentityClaims()['preferred_username'];
        console.info(`O usuário está autenticado como "${userNickname}"!`, this.oAuthService.getIdentityClaims());
      } else {
        console.warn('O usuário não está autenticado.');
      }
    });
  }

  logout() {
    this.oAuthService.logOut();
    this.router.navigate(['/']);
  }
}
