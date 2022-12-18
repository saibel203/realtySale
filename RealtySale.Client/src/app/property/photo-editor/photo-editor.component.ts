import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IPhoto } from 'src/app/models/IPhoto.interface';
import { Property } from 'src/app/models/Property';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {

  @Input() property?: Property;
  @Output() mainPhotoChangedEvent = new EventEmitter<string>();

  constructor(private housingService: HousingService) { }

  ngOnInit(): void {}

  mainPhotoChanged(url: string) {
    this.mainPhotoChangedEvent.emit(url);
  }

  setPrimaryPhoto(propertyId: number, photo: IPhoto) {
    this.housingService.setPrimaryPhoto(propertyId, photo.id).subscribe(() => {
      this.mainPhotoChanged(photo.imageUrl);
      if (this.property?.photos) {
        this.property?.photos.forEach(p => {
          if (p.isPrimary) { p.isPrimary = false; }
          if (p.id === photo.id) { p.isPrimary = true; }
        });
      }
    });
  }

  deletePhoto(propertyId: number, photo: IPhoto) {
    this.housingService.deletePhoto(propertyId, photo.id).subscribe(() => {
      if (this.property?.photos) {
        this.property.photos = this.property?.photos.filter(x => x.id !== photo.id);
      }
    });
  }

}
