import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {User} from "../models/user";
import {catchError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  signUp(user: User) {
    return this.http.post(BASE_URL+AUTH+SIGN_UP, user);
  }
}

const BASE_URL = 'http://localhost:5031'
const AUTH = '/auth'
const SIGN_UP = '/sign-up'
