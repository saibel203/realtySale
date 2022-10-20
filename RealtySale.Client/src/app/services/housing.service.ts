import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Property } from '../models/Property';

@Injectable({
  providedIn: 'root'
})
export class HousingService {
  constructor(private http: HttpClient) {}

  getAllCities(): Observable<string[]> {
    return this.http.get<string[]>('https://localhost:7232/api/city/allCities');
  }

  getProperty(id: number) {
    return this.getAllHouseProperties().pipe(
      map(propertiesArray => {
        return propertiesArray.find(p => p.Id === id) as Property;
      })
    );
  }

  getAllHouseProperties(SellRent?: number) : Observable<Property[]> {
    return this.http.get('data/properties.json').pipe(
      map((data: any) => {
        const propertiesArray: Array<Property> = [];
        const localProperties = JSON.parse(localStorage.getItem('newProperty')!);

        if (localProperties) {
          for (const id in localProperties) {
            if (SellRent === 1 || SellRent === 0) {
              if (localProperties.hasOwnProperty(id) && localProperties[id].SellRent === SellRent)
                propertiesArray.push(localProperties[id]);
            } else {
              propertiesArray.push(localProperties[id]);
            }
          }
        }

        for (const id in data) {
          if (SellRent === 1 || SellRent === 0) {
            if (data.hasOwnProperty(id) && data[id].SellRent === SellRent)
              propertiesArray.push(data[id]);
          } else {
            propertiesArray.push(data[id]);
          }
        }
        return propertiesArray;
      })
    );
  }

  addProperty(property: Property) {
    let newProp = [property];

    // Add new property in array if newProp already exists in localStorage
    if (localStorage.getItem('newProperty')) {
      newProp = [property, ...JSON.parse(localStorage.getItem('newProperty')!)];
    }
    localStorage.setItem('newProperty', JSON.stringify(newProp));
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
}
