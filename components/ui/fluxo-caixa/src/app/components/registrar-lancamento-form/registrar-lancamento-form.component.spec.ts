import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarLancamentoFormComponent } from './registrar-lancamento-form.component';

describe('RegistrarLancamentoFormComponent', () => {
  let component: RegistrarLancamentoFormComponent;
  let fixture: ComponentFixture<RegistrarLancamentoFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
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
