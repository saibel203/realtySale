import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  constructor(private alertify: AlertifyService, private router: Router) { }

  loggedinUser?: string;
  username: string = localStorage.getItem('username') as string;

  ngOnInit(): void {}

  loginToken() {
    this.loggedinUser = localStorage.getItem('username') as string;
    return this.loggedinUser;
  }

  gotoAllPropertiesUser() {
    this.router.navigate(['/user', 'all-properties', this.username]).then(() => {
      window.location.reload();
    });
  }

  onLogout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('imagePath');
    localStorage.removeItem('userData');
    this.alertify.success('You are logged out');
    this.router.navigate(['/']).then(() => window.location.reload());
  }

}
