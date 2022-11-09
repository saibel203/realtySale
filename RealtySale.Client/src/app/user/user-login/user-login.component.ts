import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserForLogin } from 'src/app/models/userForLogin.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.scss']
})
export class UserLoginComponent implements OnInit {

  constructor(private authSerice: AuthService, private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit(): void {}

  onLogin(loginForm: NgForm) {
    this.authSerice.authUser(loginForm?.value).subscribe(
      (response: UserForLogin) => {
        const user = response;

        if (user) {
          localStorage.setItem('token', user?.token);
          localStorage.setItem('username', user?.username);
          this.alertify.success("Login successfully");
          this.router.navigate(['/']);
        }
      }
    );

  }

}
