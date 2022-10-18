import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from "@angular/router";
import { catchError, Observable, of } from "rxjs";
import { Property } from "../models/Property";
import { HousingService } from "./housing.service";

@Injectable({
  providedIn: 'root'
})
export class PropertyDetailResolverService implements Resolve<Property> {
  constructor(private housingService: HousingService, private router: Router) {}

  resolve(route: ActivatedRouteSnapshot, _state: RouterStateSnapshot):
      Observable<Property> | Property | any {
      const propertyId = route.params['id'];
      return this.housingService.getProperty(+propertyId).pipe(
        catchError((error) => {
          this.router.navigate(['/']);
          console.log(error);
          return of(null);
        })
      );
    }
}
