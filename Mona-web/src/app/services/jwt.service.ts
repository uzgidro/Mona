import {Injectable} from '@angular/core';
import {CookieService} from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor(private cookieService: CookieService) {
  }

  saveToken(token: string) {
    this.cookieService.set('jwt', token, {})
  }
  getToken(): string | undefined {
    return this.cookieService.get('jwt');
  }
}
