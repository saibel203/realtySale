import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/User.interface';
import { UserForChangePassword } from '../models/UserForChangePassword.interface';
import { UserForLogin } from '../models/UserForLogin.interface';
import { UserForRegister } from '../models/UserForRegister.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  baseUrl = environment.baseApiUrl;

  authUser(user: UserForLogin) : Observable<UserForLogin> {
    return this.http.post<UserForLogin>(this.baseUrl + '/account/login', user);
  }

  registerUser(user: UserForRegister) : Observable<UserForRegister> {
    return this.http.post<UserForRegister>(this.baseUrl + '/account/register', user);
  }

  changePassword(user: UserForChangePassword) : Observable<UserForChangePassword> {
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + localStorage.getItem('token')
      })
    };

    return this.http.post<UserForChangePassword>(this.baseUrl + '/account/changePassword', user, httpOptions);
  }

  getUserData(username: string) : Observable<User> {
    return this.http.get<User>(this.baseUrl + '/account/user/' + username);
  }
}
