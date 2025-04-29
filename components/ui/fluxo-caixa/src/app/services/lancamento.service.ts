import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Lancamento } from '../models/lancamento.model';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class LancamentoService {
  private apiUrl = "http://localhost:5480/lancamentos/";
  private httpClient = inject(HttpClient);
  private authService = inject(AuthService);

  httpOptions = {
    headers: {
      'Authorization': 'Bearer ' + this.authService.getAuthorizationToken(),
    }
  }

  registrar(lancamento: Lancamento): Observable<unknown> {
    return this.httpClient.post<Lancamento>(this.apiUrl, lancamento)
  }
}
