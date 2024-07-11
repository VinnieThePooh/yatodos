import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { IUserProfile } from '../../models/user-profile';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [    
        FormsModule   
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {

  userProfile:IUserProfile = {email: '', password: ''};

  constructor(    
    private authService: AuthService,
    private router: Router    
  ) {        
  }

  login() {

    console.log("Login called")

    if (!this.userProfile.email || !this.userProfile.password)
      return;
    
      this.authService
        .login(this.userProfile.email, this.userProfile.password)
        .subscribe((r) => {
          if (r) {
            console.log('User is logged in');
            this.router.navigateByUrl('/');            
          }
        });       
    }
  

  onSubmit(e:Event) {
      e.preventDefault();
      this.login();
    }
}
