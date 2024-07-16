import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { JwtInterceptor } from './services/jwt-interceptor.service';
import { provideNativeDateAdapter } from '@angular/material/core';
import { JWT_OPTIONS, JwtHelperService, JwtModule } from '@auth0/angular-jwt';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(FormsModule),
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideAnimationsAsync('noop'),    
    provideHttpClient(
      withInterceptors([JwtInterceptor]) 
    ),
    provideNativeDateAdapter(),
    JwtHelperService,
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS }
  ]
};

//временный  костыль
export const BaseUrl = "http://localhost:5147";

//todo: refactor
export const ApiUrls = {  
  Todos: BaseUrl + "/api/todos",
  Login: BaseUrl + '/api/auth/login'
}

