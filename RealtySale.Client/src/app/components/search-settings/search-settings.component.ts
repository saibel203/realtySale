import { outputAst } from '@angular/compiler';
import { Component, EventEmitter, OnInit, Output, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-search-settings',
  templateUrl: './search-settings.component.html',
  styleUrls: ['./search-settings.component.scss']
})
export class SearchSettingsComponent implements OnInit {

  constructor(private modalService: BsModalService) { }

  modalRef?: BsModalRef;

  modalConfig = {
    animated: false
  };

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

  @Output() searchParams = new EventEmitter();


  ngOnInit(): void { }

  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, this.modalConfig);
  }

  onSortDirection() {
    if (this.SortDirection === 'desc') {
      this.SortDirection = 'asc';
    } else {
      this.SortDirection = 'desc';
    }
  }

  onCityFilter() {
    this.SearchCity = this.City;
    this.BhkCount = this.Bhk;
    this.MinimumPrice = this.MinP;
    this.MaximumPrice = this.MaxP;
    this.MinimumArea = this.MinA;
    this.MaximumArea = this.MaxA;

    this.modalRef?.hide();

    this.searchParams.emit(this.SearchCity);
    this.searchParams.emit(+this.BhkCount);
    this.searchParams.emit(+this.MinimumPrice);
    this.searchParams.emit(+this.MaximumPrice);
    this.searchParams.emit(+this.MinimumArea);
    this.searchParams.emit(+this.MaximumArea);
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

    // this.pageSize = 6;
    // this.page = 1;
  }
}
