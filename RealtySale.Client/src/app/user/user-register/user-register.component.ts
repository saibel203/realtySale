import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/user.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { UserService } from 'src/app/services/user.service';
import { PasswordMatchValidator } from 'src/shared/password-match.validator';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.scss']
})
export class UserRegisterComponent implements OnInit {

  registerForm?: FormGroup;
  user?: User;
  userSubmitted?: boolean;

  constructor(private fb: FormBuilder, private userService: UserService,
    private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      userName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(8)]],
      confirmPassword: [null, Validators.required]
    }, {
      validators: PasswordMatchValidator.mustMatch('password', 'confirmPassword')
    });
  }

  onSubmit() {
    this.userSubmitted = true;
    if (this.registerForm?.valid) {
      this.userService.addUser(this.userData());
      this.registerForm?.reset();
      this.userSubmitted = false;

      this.alertify.success('Successfully registered');
    } else {
      this.alertify.error('You have some errors');
    }
  }

  userData(): User {
    return this.user = {
      userName: this.userName?.value,
      email: this.email?.value,
      password: this.password?.value,
    };
  }

  // ------------------------------------
  // Getter methods for all form controls
  // ------------------------------------

  get userName() {
    return this.registerForm?.get('userName') as FormControl;
  }

  get email() {
    return this.registerForm?.get('email') as FormControl;
  }

  get password() {
    return this.registerForm?.get('password') as FormControl;
  }

  get confirmPassword() {
    return this.registerForm?.get('confirmPassword') as FormControl;
  }

  get passwordMathError() {
    return (
      this.registerForm?.getError('mustMatch') &&
      this.registerForm?.get('confirmPassword')?.touched
    );
  }

}
