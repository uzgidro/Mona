import {Component} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgIf} from "@angular/common";
import {RouterLink} from "@angular/router";
import {ApiService} from "../../services/api.service";
import {ConverterService} from "../../services/converter.service";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIf,
    RouterLink
  ],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {
  conflictError = false
  profileForm = new FormGroup({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    passwordRepeat: new FormControl(''),
  })
  protected readonly ErrorCode = ErrorCode

  constructor(private apiService: ApiService, private converter: ConverterService) {
  }

  onSubmit() {
    this.conflictError = false
    if (this.profileForm.valid) {
      let model = this.converter.convertUserFormToModel(this.profileForm.value);
      if (model) {
        this.apiService.signUp(model).subscribe({
          next: value => {
            // TODO(): Add Toast show on success
            console.log(value)

          },
          error: (err: HttpErrorResponse) => {
            if (err.status == 409)
              this.conflictError = true
            // TODO(): Add Toast show on error
            console.log('Ошибка, добавь вывод тоста!')
            console.log(err)
          }
        })
      }

    }
    this.profileForm.reset()


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
