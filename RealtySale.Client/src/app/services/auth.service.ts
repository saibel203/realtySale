import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IEmailBody } from '../models/IEmailBody.interface';
import { IPropertyBase } from '../models/IPropertyBase.interface';
import { UserForChangePassword } from '../models/UserForChangePassword.interface';
import { UserForFavourite } from '../models/UserForFavourite.interface';
import { UserForLogin } from '../models/UserForLogin.interface';
import { UserForProfile } from '../models/UserForProfile.interface';
import { UserForRegister } from '../models/UserForRegister.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  baseUrl = environment.baseApiUrl;

  httpOptions = {
    headers: new HttpHeaders({
      Authorization: 'Bearer ' + localStorage.getItem('token')
    })
  };

  /* --------------------------------------------------------------------------------
                                    Authentication
    -------------------------------------------------------------------------------- */

  authUser(user: UserForLogin): Observable<UserForLogin> {
    return this.http.post<UserForLogin>(this.baseUrl + '/account/login', user);
  }

  registerUser(user: UserForRegister): Observable<UserForRegister> {
    return this.http.post<UserForRegister>(this.baseUrl + '/account/register', user);
  }

  changePassword(user: UserForChangePassword): Observable<UserForChangePassword> {
    return this.http.post<UserForChangePassword>(this.baseUrl + '/account/changePassword', user, this.httpOptions);
  }

  /* --------------------------------------------------------------------------------
                                    Data manipulated
    -------------------------------------------------------------------------------- */

  getUserData(username: string): Observable<UserForProfile> {
    return this.http.get<UserForProfile>(this.baseUrl + '/account/user/' + username);
  }

  getUserDataById(id: number): Observable<UserForProfile> {
    return this.http.get<UserForProfile>(this.baseUrl + '/account/user/' + id);
  }

  changeUserData(user: UserForProfile, username: string): Observable<UserForProfile> {
    return this.http.put<UserForProfile>(this.baseUrl + '/account/profileChange/' + username, user, this.httpOptions);
  }

  changeUserImage(patchForm: any, username: string): Observable<UserForProfile> {
    return this.http.patch<UserForProfile>(this.baseUrl + '/account/profileChangeImage/' + username, patchForm, this.httpOptions);
  }

  addUserImage(formFile: FormData) {
    return this.http.post(this.baseUrl + '/account/uploadImage', formFile, this.httpOptions);
  }

  /* --------------------------------------------------------------------------------
                                    Favourite list
    -------------------------------------------------------------------------------- */

  addToFavourite(user: UserForFavourite, propertyId: number) {
    return this.http.post(this.baseUrl + '/account/addToFavourite/' + propertyId, user, this.httpOptions);
  }

  getFavouriteList(username: string): Observable<IPropertyBase[]> {
    return this.http.get<IPropertyBase[]>(this.baseUrl + '/account/AllFavourites/' + username, this.httpOptions);
  }

  isPropertyFavourite(user: UserForFavourite, propertyId: number) {
    return this.http.post(this.baseUrl + '/account/isPropertyFavourite/' + propertyId, user, this.httpOptions);
  }

  /* --------------------------------------------------------------------------------
                                        Email
    -------------------------------------------------------------------------------- */

  sendEmail(body: IEmailBody) {
    return this.http.post(this.baseUrl + '/account/sendEmail', body, this.httpOptions);
  }
}
