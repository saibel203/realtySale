import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/User.interface';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  user?: User;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.getUserData(localStorage.getItem('username')?.toString()!).subscribe(
      (data) => {
        this.user = data;
      }
    );
    console.log(this.user);
  }

}
