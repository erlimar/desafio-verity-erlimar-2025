import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideOAuthClient } from 'angular-oauth2-oidc';
import { OidcGuard } from './oidc.guard';

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
