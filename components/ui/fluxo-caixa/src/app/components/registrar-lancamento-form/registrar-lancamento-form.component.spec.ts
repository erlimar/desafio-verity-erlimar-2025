import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarLancamentoFormComponent } from './registrar-lancamento-form.component';
import { provideHttpClient } from '@angular/common/http';
import { provideOAuthClient } from 'angular-oauth2-oidc';

describe('RegistrarLancamentoFormComponent', () => {
  let component: RegistrarLancamentoFormComponent;
  let fixture: ComponentFixture<RegistrarLancamentoFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideOAuthClient()
      ],
      imports: [RegistrarLancamentoFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistrarLancamentoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
