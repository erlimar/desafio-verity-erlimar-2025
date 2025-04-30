import { TestBed } from '@angular/core/testing';

import { LancamentoService } from './lancamento.service';
import { provideHttpClient } from '@angular/common/http';
import { provideOAuthClient } from 'angular-oauth2-oidc';

describe('LancamentoService', () => {
  let service: LancamentoService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideOAuthClient()
      ]
    });
    service = TestBed.inject(LancamentoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
