import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { IEmailBody } from 'src/app/models/IEmailBody.interface';
import { Property } from 'src/app/models/Property';
import { UserForProfile } from 'src/app/models/UserForProfile.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { HousingService } from 'src/app/services/housing.service';

@Component({
  selector: 'app-property-detail',
  templateUrl: './property-detail.component.html',
  styleUrls: ['./property-detail.component.scss']
})
export class PropertyDetailComponent implements OnInit {

  @Input() slides: any;

  constructor(private route: ActivatedRoute, private housingService: HousingService,
    private modalService: BsModalService, private alertify: AlertifyService, private router: Router,
    private fb: FormBuilder, private authService: AuthService) { }

  public propertyId?: number;
  public mainPhotoUrl?: string;

  slidesImages?: { image: string; text?: string }[];

  isSliderWrap = true;
  isSliderAnimate = true;
  noHoverWrap = false;
  intervalTime = 3000;

  property?= new Property();
  modalRef?: BsModalRef;
  sendEmailListForm?: FormGroup;
  emailBody?: IEmailBody;
  userInfo?: UserForProfile;

  modalConfig = {
    animated: false
  };

  ngOnInit(): void {
    this.propertyId = +this.route.snapshot.params['id'];
    this.route.data.subscribe(
      (data: any) => {
        this.property = data['prp'];
      }
    );

    this.slidesImages = this.initSliderImages();
    this.property!.age = this.housingService.getPropertyAge(this.property?.estPossessionOn?.toString()!);

    this.getUserData();
    this.CreateSendMailForm();
  }

  changePrimaryPhoto(mainPhotoUrl: string) {
    this.mainPhotoUrl = mainPhotoUrl;
  }

  initSliderImages() {
    const photoUrls = [];

    if (this.property?.photos)
      for (let photo of this.property?.photos) {
        if (photo.isPrimary)
          this.mainPhotoUrl = photo.imageUrl;
        else
          photoUrls.push({ image: photo.imageUrl });
      }

    return photoUrls;
  }

  getUserData() {
    this.authService.getUserDataById(this.property?.postedBy!).subscribe(
      (result: UserForProfile) => {
        this.userInfo = result;
      }
    );
  }

  openModal(template: TemplateRef<any>): void {
    if (!localStorage.getItem('username')) {
      this.alertify.error('You must be logged in to add a property');
      this.router.navigate(['/user/login']);
    }

    this.modalRef = this.modalService.show(template, this.modalConfig);
  }

  onSubmit() {
    this.authService.sendEmail(this.EmailBody()).subscribe(() => {
      this.onReset();
      this.alertify.success('Congrats, letter successfully send.');
    });
  }

  onReset() {
    this.sendEmailListForm?.reset();
    this.modalRef?.hide();
  }

  EmailBody(): IEmailBody {
    const username = localStorage.getItem('username')!;

    return this.emailBody = {
      toEmail: this.userInfo?.username!,
      subject: this.Subject.value,
      content: this.Content.value,
      username: username
    };
  }

  CreateSendMailForm() {
    this.sendEmailListForm = this.fb.group({
      Subject: [null, [Validators.required, Validators.minLength(5), Validators.maxLength(150)]],
      Content: [null, [Validators.required, Validators.minLength(10), Validators.maxLength(1000)]]
    });
  }

  get Subject() {
    return this.sendEmailListForm?.get('Subject') as FormControl;
  }
  get Content() {
    return this.sendEmailListForm?.get('Content') as FormControl;
  }
}
