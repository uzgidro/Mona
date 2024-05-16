import {Component} from '@angular/core';
import {NgIf} from "@angular/common";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ApiService} from "../../services/api.service";
import {JwtService} from "../../services/jwt.service";
import {Tokens} from "../../models/tokens";
import {Router, RouterLink} from "@angular/router";
import { HttpErrorResponse } from '@angular/common/http';


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
  isLoginValid = true

  profileForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  })

  constructor(private apiService: ApiService, private jwtService: JwtService, private router: Router) {
  }

  onSubmit() {
    if (this.profileForm.valid) {
      this.apiService.signIn(this.profileForm.value).subscribe({
        next: (value: Tokens) => {
          this.jwtService.saveTokens(value);
          this.router.navigate(['/message'])
        },
        error: (err: HttpErrorResponse) => {
          if(err.status == 400) {
            this.isLoginValid = false
          }
        }
      })
    }
  }
}
