import { Injectable } from '@angular/core';
import { AuthData } from '../models/auth-data';
import { HttpClient } from '@angular/common/http';
import { catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isLoggedIn: boolean = false;

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<AuthData>('/api/login', {email, password}).pipe(
      map((response) => {
        localStorage.setItem('JWT_Token', response.token);
        this.isLoggedIn = true;
        return true;
      }),
      catchError((error) => {
        console.log(error);
        this.isLoggedIn = false;
        return of(false);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('JWT_Token');
    this.isLoggedIn = false;
  }

  isAuthenticated(): boolean {
    return this.isLoggedIn;
  }
}
