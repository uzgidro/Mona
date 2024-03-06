import {Component} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgIf} from "@angular/common";
import {ConverterService} from "../services/converter.service";
import {ApiService} from "../services/api.service";

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {
  profileForm = new FormGroup({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    personalId: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    passwordRepeat: new FormControl(''),
  })
  protected readonly ErrorCode = ErrorCode

  constructor(private apiService: ApiService, private converter: ConverterService) {
  }

  onSubmit() {
    if (this.profileForm.valid) {
      let model = this.converter.convertUserFormToModel(this.profileForm.value);
      if (model) {
        this.apiService.signUp(model).subscribe({
          next: value => {
            console.log(value)
          }
        })
      }

    }
  }

  onPasswordInput() {
    if (this.profileForm.get('password')?.value !== this.profileForm.get('passwordRepeat')?.value) {
      this.profileForm.setErrors({
        passwordIdentity: 'passwords are not the same'
      })
    }
  }
}

enum ErrorCode {
  REQUIRED = 'required',
  MIN_LENGTH = 'minlength',
  PASSWORD_IDENTITY = 'passwordIdentity',
}
