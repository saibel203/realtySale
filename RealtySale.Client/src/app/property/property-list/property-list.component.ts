import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IPropertyBase } from 'src/app/models/iPropertyBase.interface';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-property-list',
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.scss']
})
export class PropertyListComponent implements OnInit {
  constructor(private route: ActivatedRoute, private housingService: HousingService) { }

  SellRent: number = 0;
  properties?: IPropertyBase[];
  City = '';
  SearchCity = '';
  SortbyParam = '';
  SortDirection = 'asc';

  ngOnInit(): void {
    if (this.route.snapshot.url.toString()) {
      this.SellRent = 1;
    }
    this.housingService.getAllHouseProperties(this.SellRent).subscribe(
      data => {
        this.properties = data;
      }
    );
  }

  onCityFilter() {
    this.SearchCity = this.City;
  }

  onCityFilterClear() {
    this.SearchCity = '';
    this.City = '';
  }

  onSortDirection() {
    if (this.SortDirection === 'desc') {
      this.SortDirection = 'asc';
    } else {
      this.SortDirection = 'desc';
    }
  }
}
