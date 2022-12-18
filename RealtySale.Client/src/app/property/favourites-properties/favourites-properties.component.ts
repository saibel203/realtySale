import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IPropertyBase } from 'src/app/models/IPropertyBase.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-favourites-properties',
  templateUrl: './favourites-properties.component.html',
  styleUrls: ['./favourites-properties.component.scss']
})
export class FavouritesPropertiesComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router,
    private alertify: AlertifyService) { }

  properties?: IPropertyBase[];

  ngOnInit(): void {
    if (!localStorage.getItem('username')) {
      this.alertify.error('You must be logged in to add a property');
      this.router.navigate(['/user/login']);
    }

    this.getFavouriteProperties();
  }

  getFavouriteProperties() {
    const username = localStorage.getItem('username')!;

    this.authService.getFavouriteList(username).subscribe(
      (result: IPropertyBase[]) => {
        this.properties = result;
      }
    );
  }

}
