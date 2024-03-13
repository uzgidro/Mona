import {Component} from '@angular/core';
import {NgIf} from "@angular/common";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ApiService} from "../services/api.service";
import {JwtService} from "../services/jwt.service";

@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.css'
})
export class SignInComponent {
  profileForm = new FormGroup({
    personalId: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  })

  constructor(private apiService: ApiService,private jwtService: JwtService) {
  }

  onSubmit() {
    if (this.profileForm.valid) {
      this.apiService.signIn(this.profileForm.value).subscribe({
        next: (value: {accessToken: string, refreshToken: string}) => {
          console.log(value)
          this.jwtService.saveToken(value.accessToken);
          this.jwtService.saveToken(value.refreshToken);

          const token = this.jwtService.getToken();
          console.log('Token from cookie:', token);
        }

      })
    }
  }
}
