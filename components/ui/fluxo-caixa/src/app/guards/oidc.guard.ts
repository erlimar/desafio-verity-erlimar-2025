import { inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  GuardResult,
  MaybeAsync,
  RouterStateSnapshot
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class OidcGuard implements CanActivate {
  private authService = inject(AuthService);

  /* eslint-disable  @typescript-eslint/no-unused-vars */
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): MaybeAsync<GuardResult> {

    if (this.authService.isAuthenticated()) {
      return true;
    }

    return false;
  }
  /* eslint-enable  @typescript-eslint/no-unused-vars */
}
