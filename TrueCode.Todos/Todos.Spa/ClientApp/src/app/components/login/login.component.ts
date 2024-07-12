import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { IUserCredentials, IUserProfile } from '../../models/user-models';
import { FormsModule } from '@angular/forms';
import { UserProfileService } from '../../services/user-profile.service';

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

  userCredentials:IUserCredentials = {email: '', password: ''};

  constructor(
    private profileService: UserProfileService,    
    private authService: AuthService,
    private router: Router    
  ) {        
  }

  login() {

    console.log("Login called")

    if (!this.userCredentials.email || !this.userCredentials.password)
      return;
    
      this.authService
        .login(this.userCredentials.email, this.userCredentials.password)
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
