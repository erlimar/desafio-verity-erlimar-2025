import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarLancamentoPageComponent } from './registrar-lancamento-page.component';
import { provideHttpClient } from '@angular/common/http';
import { provideOAuthClient } from 'angular-oauth2-oidc';

describe('RegistrarLancamentoPageComponent', () => {
  let component: RegistrarLancamentoPageComponent;
  let fixture: ComponentFixture<RegistrarLancamentoPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideOAuthClient()
      ],
      imports: [RegistrarLancamentoPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrarLancamentoPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
