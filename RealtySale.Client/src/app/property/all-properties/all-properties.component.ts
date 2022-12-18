import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IPropertyBase } from 'src/app/models/IPropertyBase.interface';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-all-properties',
  templateUrl: './all-properties.component.html',
  styleUrls: ['./all-properties.component.scss']
})
export class AllPropertiesComponent implements OnInit {

  constructor(private propertyService: HousingService, private activaterRouter: ActivatedRoute) { }

  properties?: IPropertyBase[];
  username?: string;

  ngOnInit(): void {
    console.log(this.username);
    this.getAllProperties();
  }

  getAllProperties() {
    this.username = this.activaterRouter.snapshot.paramMap.get('username')!;
    this.propertyService.getAllUserProperties(this.username!).subscribe(
      (data: IPropertyBase[]) => {
        this.properties = data;
        console.log(data);
      }
    );
  }
}
