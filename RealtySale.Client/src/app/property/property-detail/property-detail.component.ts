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
  galleryImages?: any;

  constructor(private route: ActivatedRoute, private router: Router,
    private housingService: HousingService) {}

  public propertyId?: number;

  ngOnInit(): void {
    this.propertyId = +this.route.snapshot.params['id'];
    this.route.data.subscribe(
      (data) => {
        this.property = data['prp'];
        console.log(this.property?.photos);
      }
    );

    this.property!.age = this.housingService.getPropertyAge(this.property?.estPossessionOn?.toString()!);
    this.galleryImages = this.getPropertyPhotos();
  }

  getPropertyPhotos() {
    const photoUrls = [];
    if (this.property?.photos)
      for (const photo of this.property?.photos) {
        photoUrls.push({ image: photo?.imageUrl });
      }

    return photoUrls;
  }

}
