import { File, MessageModel } from './../models/message';
import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {User} from "../models/user";
import {Observable, catchError, map, throwError} from "rxjs";
import {JwtService} from "./jwt.service";
import {Tokens} from "../models/tokens";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient, private jwt: JwtService) {
  }

  signUp(user: User) {
    return this.http.post(BASE_URL + AUTH + SIGN_UP, user);
  }

  signIn(user: Partial<any>): Observable<any> {
    return this.http.post(BASE_URL + AUTH + SIGN_IN, user);
  }

  async refreshTokens() {
    const oldToken: Tokens = {accessToken: this.jwt.getAccessToken(), refreshToken: this.jwt.getRefreshToken()}
    try {
      return await this.http.post<Tokens>(BASE_URL + AUTH + REFRESH, oldToken)
        .forEach((value: Tokens) => {
          this.jwt.saveTokens(value)
        });
    } catch (err) {
      return console.log(err);
    }
  }

  authCheck(){
    return this.http.get(BASE_URL+USERS+LIST)
  }


  sendMessage(formData:FormData){
    console.log(formData.get('file'));
    return this.http.post(BASE_URL+MESSAGE+SEND,formData).subscribe({
      next:(values:any)=>console.log(values),
      error:err=>console.log(err),
      complete:()=>{
        console.log('Completed');
      }
    })
  }

  downloadFile( file: File) {
    const url=BASE_URL+FILES+DOWNLOAD+'/'+file.id;
    const headers = new HttpHeaders();
    headers.set('Accept', 'application/octet-stream');
    this.http.get(url, { headers: headers, responseType: 'blob' }).subscribe((response: Blob) => {
      this.handleResponse(response, file.name);
    });
  }


  getUserInfo(ID:string){
    return this.http.get(BASE_URL+USERS+'/'+ID)
  }

  private handleResponse(blob: Blob, filename: string) {
    const downloadLink = document.createElement('a');
    const url = window.URL.createObjectURL(blob);
    downloadLink.href = url;
    downloadLink.download = filename;
    document.body.appendChild(downloadLink);
    downloadLink.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(downloadLink);
  }
}


const BASE_URL = 'http://127.0.0.1:5031'
const AUTH = '/auth'
const SIGN_UP = '/sign-up'
const SIGN_IN = '/sign-in'
const REFRESH = '/refresh'
const USERS='/user'
const LIST='/list'
const MESSAGE='/message'
const SEND='/send'
const DOWNLOAD='/download'
const FILES='/files'




