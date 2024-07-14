import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { JwtInterceptor } from './services/jwt-interceptor.service';
import { provideNativeDateAdapter } from '@angular/material/core';

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(FormsModule),
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideAnimationsAsync('noop'),    
    provideHttpClient(
      withInterceptors([JwtInterceptor])
    ),
    provideNativeDateAdapter()
  ]
};

export const BaseUrl = "http://localhost:5146";

//todo: refactor
export const ApiUrls = {  
  Todos: BaseUrl + "/api/todos"
}

