import { Injectable } from '@angular/core';
import { IAuthResponse } from '../models/auth-response';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { ApiUrls } from '../app.config';
import { UserProfileService } from './user-profile.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isLoggedIn: boolean = false;

  constructor(
    private profileService: UserProfileService,
    private http: HttpClient
  ) {}

  login(email: string, password: string): Observable<boolean> {
    return this.http
      .post<IAuthResponse>(ApiUrls.Login, { email, password })
      .pipe(
        map((response) => {
          localStorage.setItem('JWT_Token', response.token);
          this.profileService.setUserProfile(response.profile);
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
