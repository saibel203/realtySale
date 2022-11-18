import { Component, OnInit, Input } from '@angular/core';
import { IPropertyBase } from 'src/app/models/IPropertyBase.interface';

@Component({
  selector: 'app-property-card',
  templateUrl: './property-card.component.html',
  styleUrls: ['./property-card.component.scss']
})
export class PropertyCardComponent implements OnInit {

  constructor() { }

  @Input() property?: IPropertyBase;
  @Input() hideIcons?: boolean;

  ngOnInit(): void {
  }

}
