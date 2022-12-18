import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent implements OnInit {

  constructor() { }

  page: number = 1;
  count: number = 0;
  pageSize: number = 6;

  ngOnInit(): void {}

}
