import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { throwIfEmpty } from 'rxjs';
import { UserForProfile } from 'src/app/models/UserForProfile.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  @ViewChild('hoverImageElement', { read: ElementRef, static: false }) hoverImage?: ElementRef;
  @ViewChild('imageUpload', { read: ElementRef, static: false }) imageUpload?: ElementRef;

  constructor(private authService: AuthService, private alertify: AlertifyService,
    private router: Router, private modalService: BsModalService,
    private fb: FormBuilder) { }

  userProfile?: UserForProfile;
  modalRef?: BsModalRef;
  changeUserDataForm?: FormGroup;
  userSubmitted?: boolean;
  uploadFile?: File | null;
  userId?: number;

  modalConfig = {
    animated: false
  };

  ngOnInit() {
    if (!localStorage.getItem('username')) {
      this.alertify.error('You must be logged in to add a property');
      this.router.navigate(['/user/login']);
    }

    this.initialUserProfile();
    this.CreateUserDataChangeForm();
  }

  initialUserProfile() {
    const username = localStorage.getItem('username')!;
    this.authService.getUserData(username).subscribe(
      (data: UserForProfile) => {
        this.userProfile = data;
        this.userId = data.id;
        this.userProfile!.userImage = localStorage.getItem('imagePath')!;
        localStorage.setItem('userData', JSON.stringify(data));
      }
    );
  }

  onImageHover() {
    this.hoverImage?.nativeElement.classList.remove('hover');
  }

  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, this.modalConfig);
  }

  onSubmit() {
    this.userSubmitted = true;
    const username = localStorage.getItem('username')!;

    this.authService.changeUserData(this.UserData(), username).subscribe(
      (response: UserForProfile) => {
        this.userProfile = response;
        this.alertify.success('Congrats, you are successfully changed profile data');
        this.modalRef?.hide();
        localStorage.removeItem('userData');
        localStorage.setItem('userData', JSON.stringify(response));
      }
    );
  }

  onReset() {
    this.userSubmitted = false;
    this.changeUserDataForm?.reset();
  }

  onImageUpload(file: any) {
    let fileToUpload = <File>file[0];
    const formData = new FormData();
    const randomString = Math.floor(Math.random() * 1000);

    formData.append('image', fileToUpload);
    formData.append('imageName', fileToUpload.name.slice(0, 20) + randomString);

    this.authService.addUserImage(formData).subscribe(
      (result: any) => {
        this.alertify.success('Image was added successfully');
        localStorage.setItem('imagePath', result.message);

        const patchBody = [{
          "op": "replace",
          "path": "/userImage",
          "value": localStorage.getItem('imagePath')
        }];

        this.authService.changeUserImage(patchBody, localStorage.getItem('username')!).subscribe();
        this.initialUserProfile();
      }
    );
  }

  onImageUploadView() {
    this.imageUpload?.nativeElement.click();
  }

  CreateUserDataChangeForm() {
    const userData = JSON.parse(localStorage.getItem('userData')!);

    this.changeUserDataForm = this.fb.group({
      Email: [userData['email'], Validators.required],
      FirstName: [userData['firstName']],
      LastName: [userData['lastName']],
      TelegramLink: [userData['telegramLink']],
      InstagramLink: [userData['instagramLink']],
      FacebookLink: [userData['facebookLink']],
      Phone: [userData['phone']],
      Description: [userData['description']],
    });
  }

  UserData(): UserForProfile {
    return this.userProfile = {
      id: this.userId!,
      username: localStorage.getItem('username')!,
      email: this.Email?.value,
      firstName: this.FirstName?.value,
      lastName: this.LastName?.value,
      telegramLink: this.TelegramLink?.value,
      instagramLink: this.InstagramLink?.value,
      facebookLink: this.FacebookLink?.value,
      phone: this.Phone?.value,
      description: this.Description?.value,
      userImage: localStorage.getItem('imagePath')!
    };
  }

  // #region <Getter Methods>
  get Email() {
    return this.changeUserDataForm?.controls?.['Email'] as FormControl;
  }

  get FirstName() {
    return this.changeUserDataForm?.controls?.['FirstName'] as FormControl;
  }

  get LastName() {
    return this.changeUserDataForm?.controls?.['LastName'] as FormControl;
  }

  get TelegramLink() {
    return this.changeUserDataForm?.controls?.['TelegramLink'] as FormControl;
  }

  get InstagramLink() {
    return this.changeUserDataForm?.controls?.['InstagramLink'] as FormControl;
  }

  get FacebookLink() {
    return this.changeUserDataForm?.controls?.['FacebookLink'] as FormControl;
  }

  get Phone() {
    return this.changeUserDataForm?.controls?.['Phone'] as FormControl;
  }

  get Description() {
    return this.changeUserDataForm?.controls?.['Description'] as FormControl;
  }

  // #endregion
}
