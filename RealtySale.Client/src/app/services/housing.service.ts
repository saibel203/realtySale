import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Property } from '../models/Property';
import { environment } from 'src/environments/environment';
import { IKeyValuePair } from '../models/IKeyValuePair.interface';
import { City } from '../models/City';

@Injectable({
  providedIn: 'root'
})
export class HousingService {
  baseUrl = environment.baseApiUrl;

  constructor(private http: HttpClient) {}

  /* -----------------------------------------------------------
                              City
    -----------------------------------------------------------*/

  getAllCities(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + '/city/getAll');
  }

  addNewCity(city: City): Observable<City> {
    return this.http.post<City>(this.baseUrl + '/city/new', city);
  }

  /* -----------------------------------------------------------
                            Property
    -----------------------------------------------------------*/

  getAllPropertyTypes(): Observable<IKeyValuePair[]> {
    return this.http.get<IKeyValuePair[]>(this.baseUrl + '/propertyType/list');
  }

  getAllFurnishingTypes(): Observable<IKeyValuePair[]> {
    return this.http.get<IKeyValuePair[]>(this.baseUrl + '/furnishingType/list');
  }

  getAllHouseProperties(SellRent?: number) : Observable<Property[]> {
    return this.http.get<Property[]>(this.baseUrl + '/property/list/' + SellRent?.toString());
  }

  getProperty(id: number) : Observable<Property> {
    return this.http.get<Property>(this.baseUrl + '/property/detail/' + id.toString());
  }

  addProperty(property: Property) {
    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + localStorage.getItem('token')
      })
    };

    return this.http.post(this.baseUrl + '/property/add', property, httpOptions);
  }

  newPropertyId() {
    if (localStorage.getItem('PID')) {
      localStorage.setItem('PID', String(+localStorage.getItem('PID')! + 1));
      return +localStorage.getItem('PID')!;
    } else {
      localStorage.setItem('PID', '101');
      return 101;
    }
  }

  getPropertyAge(dateofEstablishment: string): string
    {
        const today = new Date();
        const estDate = new Date(dateofEstablishment);
        let age = today.getFullYear() - estDate.getFullYear();
        const m = today.getMonth() - estDate.getMonth();

        // Current month smaller than establishment month or
        // Same month but current date smaller than establishment date
        if (m < 0 || (m === 0 && today.getDate() < estDate.getDate())) {
            age --;
        }

        // Establshment date is future date
        if(today < estDate) return '0';

        // Age is less than a year
        if(age === 0) return 'Less than a year';

        return age.toString();
    }
}
