import { Component, inject } from '@angular/core';
import { RegistrarLancamentoFormComponent } from "../../components/registrar-lancamento-form/registrar-lancamento-form.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-registrar-lancamento-page',
  imports: [RegistrarLancamentoFormComponent],
  templateUrl: './registrar-lancamento-page.component.html',
  styleUrl: './registrar-lancamento-page.component.css',
})
export class RegistrarLancamentoPageComponent {
  private router: Router = inject(Router);

  onFormSaved() {
    alert('Lançamento criado com sucesso!');
    this.router.navigate(['/']);
  }
}
