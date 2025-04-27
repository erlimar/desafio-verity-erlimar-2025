import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  GuardResult,
  MaybeAsync,
  RouterStateSnapshot
} from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class OidcGuard implements CanActivate {
  constructor(private authService: AuthService) { }

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
