import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserForChangePassword } from 'src/app/models/UserForChangePassword.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { PasswordMatchValidator } from 'src/shared/password-match.validator';

@Component({
  selector: 'app-user-change-password',
  templateUrl: './user-change-password.component.html',
  styleUrls: ['./user-change-password.component.scss']
})
export class UserChangePasswordComponent implements OnInit {

  changePasswordForm?: FormGroup;
  userSubmitted?: boolean;
  user?: UserForChangePassword;

  constructor(private alertify: AlertifyService, private router: Router, private fb: FormBuilder,
    private authService: AuthService) { }

  ngOnInit(): void {
    if (!localStorage.getItem('username')) {
      this.alertify.error('You must be logged in to add a property');
      this.router.navigate(['/user/login']);
    }

    this.CreateChangePasswordForm();
  }

  CreateChangePasswordForm() {
    this.changePasswordForm = this.fb.group({
      oldPassword: [null, Validators.required],
      newPassword: [null, [Validators.required, Validators.minLength(8)]],
      confirmNewPassword: [null, Validators.required]
    }, {
      validators: PasswordMatchValidator.mustMatch('newPassword', 'confirmNewPassword')
    });
  }

  onSubmit() {
    this.userSubmitted = true;

    if (this.changePasswordForm?.valid) {
      this.authService.changePassword(this.userData()).subscribe(
        () => {
          this.onReset();
          this.alertify.success('Password was changed successfully');
          this.router.navigate(['/']);
        }
      );
    }
  }

  onReset() {
    this.userSubmitted = false;
    this.changePasswordForm?.reset();
  }

  userData() : UserForChangePassword {
    return this.user = {
      username: localStorage.getItem('username')!,
      password: this.oldPassword?.value,
      newPassword: this.newPassword?.value
    };
  }

  // ------------------------------------
  // Getter methods for all form controls
  // ------------------------------------

  get oldPassword() {
    return this.changePasswordForm?.controls['oldPassword'] as FormControl;
  }

  get newPassword() {
    return this.changePasswordForm?.controls['newPassword'] as FormControl;
  }

  get confirmNewPassword() {
    return this.changePasswordForm?.controls['confirmNewPassword'] as FormControl;
  }

  get passwordMathError() {
    return (
      this.changePasswordForm?.getError('mustMatch') &&
      this.changePasswordForm?.controls['confirmNewPassword']?.touched
    );
  }
}
