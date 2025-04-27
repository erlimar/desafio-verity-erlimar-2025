import { TestBed } from '@angular/core/testing';
import { OidcGuard } from './oidc.guard';
import { provideHttpClient } from '@angular/common/http';
import { provideOAuthClient } from 'angular-oauth2-oidc';

describe('OidcGuard', () => {
  let guard: OidcGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideOAuthClient()
      ]
    });
    guard = TestBed.inject(OidcGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
