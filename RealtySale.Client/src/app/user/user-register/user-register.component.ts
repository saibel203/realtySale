import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserForRegister } from 'src/app/models/UserForRegister.interface';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { PasswordMatchValidator } from 'src/shared/password-match.validator';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.scss']
})
export class UserRegisterComponent implements OnInit {

  @ViewChild('passwordInput', { read: ElementRef, static: false }) passwordInput?: ElementRef;

  registerForm?: FormGroup;
  user?: UserForRegister;
  userSubmitted?: boolean;
  showPassword: boolean = false;
  typePassword: string = 'password';

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService,
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
      this.authService.registerUser(this.userData()).subscribe(
        () => {
          this.onReset();
          this.alertify.success('Congrats, you are successfully registered');
          this.router.navigate(['/user/login']);
        }
      );
    }
  }

  onReset() {
    this.userSubmitted = false;
    this.registerForm?.reset();
  }

  userData(): UserForRegister {
    return this.user = {
      username: this.userName?.value,
      email: this.email?.value,
      password: this.password?.value
    };
  }

  onPasswordToggle() {
    this.showPassword = !this.showPassword;
    if (this.typePassword === 'password')
      this.typePassword = 'text';
    else this.typePassword = 'password';
  }

  generateRandomPassword() {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!\"$%&/()=?@~`\\.\';:+=^*_-0123456789';
    let output = this.passwordInput?.nativeElement;

    function randomValue(value: number) {
      return Math.floor(Math.random() * value);
    }

    const length = 10;
    let str = '';

    for (let i = 0; i < length; i++) {
      let random = randomValue(characters.length);
      str += characters.charAt(random);
    }

    output.value = str;

    this.registerForm?.patchValue({ 'password': str });
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
