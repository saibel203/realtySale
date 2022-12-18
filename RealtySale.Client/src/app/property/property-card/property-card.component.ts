import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IPropertyBase } from 'src/app/models/IPropertyBase.interface';
import { UserForProfile } from 'src/app/models/UserForProfile.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-property-card',
  templateUrl: './property-card.component.html',
  styleUrls: ['./property-card.component.scss']
})
export class PropertyCardComponent implements OnInit {

  @Input() property?: IPropertyBase;
  @Input() hideIcons?: boolean;

  constructor(private authService: AuthService, private alertify: AlertifyService,
    private router: Router) { }

  createdUser?: UserForProfile;
  currentUser?: UserForProfile;

  isFavourite: boolean = false;

  ngOnInit(): void {
    if (this.router.url !== '/add-property') {
      this.getUserData();

      if (localStorage.getItem('username'))
        this.isPropertyFavourite();
    }

    console.log(this.property);
  }

  getUserData() {
    this.authService.getUserDataById(this.property?.postedBy!).subscribe(
      (result: UserForProfile) => {
        this.createdUser = result;
      }
    );
  }

  addToFavourite() {
    if (!localStorage.getItem('username')) {
      this.alertify.error('You must be logged in to add a property');
      this.router.navigate(['/user/login']);
    }

    const body = {
      'username': localStorage.getItem('username')!
    };

    this.authService.addToFavourite(body, this.property?.id!).subscribe(
      (res: any) => {
        this.alertify.success(res.resultMessage);
      });

    this.isFavourite = !this.isFavourite;
  }

  isPropertyFavourite() {
    const body = {
      'username': localStorage.getItem('username')!
    };

    this.authService.isPropertyFavourite(body, this.property?.id!).subscribe(
      (res: any) => {
        if (res.isFavourite) this.isFavourite = true;
      }
    );
  }

}
