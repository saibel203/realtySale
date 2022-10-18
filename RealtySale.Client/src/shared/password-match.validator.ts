import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class PasswordMatchValidator {
  constructor() {}

  static mustMatch(source: string, target: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

      const sourcePassword = control.get(source);
      const targetPassword = control.get(target);

      return sourcePassword && targetPassword && sourcePassword?.value !== targetPassword?.value
        ? { mustMatch: true } : null;
    };
  }
}
