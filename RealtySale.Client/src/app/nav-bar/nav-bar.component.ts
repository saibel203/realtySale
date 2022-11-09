import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  constructor(private alertify: AlertifyService) { }

  loggedinUser?: string;

  ngOnInit(): void {}

  loginToken() {
    this.loggedinUser = localStorage.getItem('username') as string;
    return this.loggedinUser;
  }

  onLogout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    this.alertify.success('You are logged out');
  }

}
