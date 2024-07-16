import { Injectable } from '@angular/core';
import { IAuthResponse } from '../models/auth-response';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { ApiUrls } from '../app.config';
import { UserProfileService } from './user-profile.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { IUserProfile } from '../models/user-models';

@Injectable({
  providedIn: 'root',    
})
export class AuthService {
  constructor(
    private jwtHelper: JwtHelperService,
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
          return true;
        }),
        catchError((error) => {
          console.log(error);
          return of(false);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('JWT_Token');
    this.profileService.clearUserProfile();
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('JWT_Token');
    const isValid = !this.jwtHelper.isTokenExpired(token);
    if (isValid)
      {
        if (!this.profileService.UserProfile)
          {
            var profile = localStorage.getItem('userProfile');
            if (profile)
              this.profileService.setUserProfile(JSON.parse(profile) as IUserProfile)
          }        
      }
    return isValid;
  }
}
