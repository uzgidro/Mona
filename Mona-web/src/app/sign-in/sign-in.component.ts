import {Component} from '@angular/core';
import {NgIf} from "@angular/common";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ApiService} from "../services/api.service";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.css'
})
export class SignInComponent {
  profileForm = new FormGroup({
    personalId: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  })

  constructor(private apiService: ApiService) {
  }

  onSubmit() {
    if (this.profileForm.valid) {
      this.apiService.signIn(this.profileForm.value).subscribe({
        next: value => {
          console.log(value)
        }
      })
    }
  }
}
