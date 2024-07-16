import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTreeModule } from '@angular/material/tree';
import { RouterLink } from '@angular/router';
import { MatNavList } from '@angular/material/list';
import { CommonModule } from '@angular/common';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,    
    MatSidenavModule, 
    MatNavList,
    MatTreeModule,
    RouterLink,        
  ],  
  providers: [
    JwtHelperService
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'ClientApp';
}
