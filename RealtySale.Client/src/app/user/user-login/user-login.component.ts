import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserForLogin } from 'src/app/models/UserForLogin.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.scss']
})
export class UserLoginComponent implements OnInit {

  showPassword: boolean = false;
  typePassword: string = 'password';

  constructor(private authSerice: AuthService, private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit(): void {
    if (localStorage.getItem('username')) {
      this.alertify.error('You have already been authenticated');
      this.router.navigate(['/']);
    }
  }

  onLogin(loginForm: NgForm) {
    this.authSerice.authUser(loginForm?.value).subscribe(
      (response: UserForLogin) => {
        const user = response;

        if (user) {
          localStorage.setItem('token', user?.token);
          localStorage.setItem('username', user?.username);
          localStorage.setItem('imagePath', user?.userImage);

          this.authSerice.getUserData(user?.username).subscribe(
            (response) => {
              localStorage.setItem('userData', JSON.stringify(response));
            }
          );
          this.alertify.success("Login successfully");
          this.router.navigate(['/']).then(() => window.location.reload());
        }
      }
    );
  }

  onPasswordToggle() {
    this.showPassword = !this.showPassword;
    if (this.typePassword === 'password')
      this.typePassword = 'text';
    else this.typePassword = 'password';
  }

}
