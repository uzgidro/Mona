import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {User} from "../models/user";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) {
  }

  signUp(user: User) {
    return this.http.post(BASE_URL + AUTH + SIGN_UP, user);
  }

  signIn(user: Partial<any>): Observable<any> {
    return this.http.post(BASE_URL + AUTH + SIGN_IN, user);
  }
}

const BASE_URL = 'http://localhost:5031'
const AUTH = '/auth'
const SIGN_UP = '/sign-up'
const SIGN_IN = '/sign-in'
