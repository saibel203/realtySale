import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserForLogin } from '../models/userForLogin.interface';
import { UserForRegister } from '../models/userForRegister.interface';

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
}
