import { Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { AboutPageComponent } from './about-page/about-page.component';
import { OidcGuard } from './oidc.guard';

export const routes: Routes = [
  {
    path: '',
    component: HomePageComponent
  },
  {
    path: 'about',
    component: AboutPageComponent,
    canActivate: [OidcGuard]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
