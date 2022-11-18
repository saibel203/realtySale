import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { IPropertyBase } from 'src/app/models/IPropertyBase.interface';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-property-list',
  templateUrl: './property-list.component.html',
  styleUrls: ['./property-list.component.scss']
})
export class PropertyListComponent implements OnInit {
  constructor(private route: ActivatedRoute, private housingService: HousingService,
    private modalService: BsModalService) { }

  modalRef?: BsModalRef;

  modalConfig = {
    animated: false
  };

  SellRent: number = 0;
  properties?: IPropertyBase[];
  SortbyParam = '';
  SortDirection = 'asc';

  City = '';
  Bhk = 0;
  MinP = 0;
  MaxP = Infinity;
  MinA = 0;
  MaxA = Infinity;

  SearchCity = '';
  BhkCount = 0;
  MinimumPrice = 0;
  MaximumPrice = Infinity;
  MinimumArea = 0;
  MaximumArea = Infinity;

  page: number = 1;
  count: number = 0;
  pageSize: number = 6;

  ngOnInit(): void {
    if (this.route.snapshot.url.toString()) {
      this.SellRent = 1;
    }
    this.getList();
  }

  getList(): void {
    this.housingService.getAllHouseProperties(this.SellRent).subscribe(
      data => {
        this.properties = data;
      }
    );
  }

  onCityFilter() {
    this.SearchCity = this.City;
    this.BhkCount = this.Bhk;
    this.MinimumPrice = this.MinP;
    this.MaximumPrice = this.MaxP;
    this.MinimumArea = this.MinA;
    this.MaximumArea = this.MaxA;

    this.modalRef?.hide();
  }

  onCityFilterClear() {
    this.SearchCity = '';
    this.City = '';

    this.BhkCount = 0;
    this.Bhk = 0;

    this.MinimumPrice = 0;
    this.MinP = 0;

    this.MaximumPrice = Infinity;
    this.MaxP = Infinity;

    this.MinimumArea = 0;
    this.MinA = 0;

    this.MaximumArea = Infinity;
    this.MaxA = Infinity;

    this.pageSize = 6;
    this.page = 1;
}

  onSortDirection() {
    if (this.SortDirection === 'desc') {
      this.SortDirection = 'asc';
    } else {
      this.SortDirection = 'desc';
    }
  }

  onListDataChange(event: any): void {
    this.page = event;
    this.getList();
  }

  onListSizeChange(event: any): void {
    this.pageSize = event.target.value;

    this.page = 1;
    this.getList();
  }

  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, this.modalConfig);
  }
}
