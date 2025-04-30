import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Output, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LancamentoService } from '../../services/lancamento.service';
import { catchError, of } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-registrar-lancamento-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './registrar-lancamento-form.component.html',
  styleUrl: './registrar-lancamento-form.component.css'
})
export class RegistrarLancamentoFormComponent implements OnInit {
  private lancamentoService = inject(LancamentoService);

  @Output() formSaved = new EventEmitter<void>();

  form!: FormGroup
  sending = false;
  generalErrors: string[] = [];

  ngOnInit(): void {
    this.form = new FormGroup({
      dataHora: new FormControl('', [Validators.required]),
      valor: new FormControl('', [Validators.required]),
      descricao: new FormControl('', [Validators.required])
    });
  }

  get f() {
    return this.form.controls;
  }

  submit() {
    this.beforeSending();

    this.lancamentoService.registrar(this.form.value)
      .pipe(catchError((response: HttpErrorResponse) => {
        const errors: string[] = [];

        if (response?.error && Array.isArray(response?.error)) {
          response.error.forEach((error) => {
            errors.push(error?.errorMessage || error || 'Houve um erro inesperado durante a chamada da API');
          });
        } else {
          errors.push('Houve um erro inesperado durante a chamada da API');
        }

        this.afterSending(errors);

        return of();
      }))
      .subscribe({
        next: () => {
          this.formSaved.emit();
          this.afterSending([]);
        },
        error: (error) => {
          this.afterSending([error?.message || error || 'Houve um erro inesperado ao chamar a API']);
        }
      });
  }

  beforeSending() {
    this.sending = true;
    this.generalErrors = [];
  }

  afterSending(errors: string[]) {
    this.sending = false;
    this.generalErrors = errors;
  }
}
