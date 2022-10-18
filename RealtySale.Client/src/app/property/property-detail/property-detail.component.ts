import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Property } from 'src/app/models/Property';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-property-detail',
  templateUrl: './property-detail.component.html',
  styleUrls: ['./property-detail.component.scss']
})
export class PropertyDetailComponent implements OnInit {

  property? = new Property();
  galleryImages = [
    { image: 'assets/images/house_1-detail-1.jpg' },
    { image: 'assets/images/house_1-detail-2.jpg' },
    { image: 'assets/images/house_1-detail-3.jpg' },
    { image: 'assets/images/house_1-detail-4.jpg' }
  ];

  constructor(private route: ActivatedRoute, private router: Router,
    private housingService: HousingService) {}

  public propertyId?: number;

  ngOnInit(): void {
    this.propertyId = +this.route.snapshot.params['id'];
    this.route.data.subscribe(
      (data) => {
        this.property = data['prp'];
      }
    );
  }

}
