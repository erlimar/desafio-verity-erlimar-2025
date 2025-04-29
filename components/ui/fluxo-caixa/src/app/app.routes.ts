import { Routes } from '@angular/router';
import { RegistrarLancamentoPageComponent } from './pages/registrar-lancamento-page/registrar-lancamento-page.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { OidcGuard } from './guards/oidc.guard';

export const routes: Routes = [
  {
    path: '',
    component: HomePageComponent
  },
  {
    path: 'registrar-lancamento',
    component: RegistrarLancamentoPageComponent,
    canActivate: [OidcGuard]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
