import {Injectable} from '@angular/core';
import {CookieService} from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor(private cookieService: CookieService) {
  }

  saveTokens(token: { accessToken: string, refreshToken: string }) {
    this.cookieService.set('jwt_access', token.accessToken, {})
    this.cookieService.set('jwt_refresh', token.refreshToken, {})

  }

  getAccessToken(): string {
    return this.cookieService.get('jwt_access')
  }

  getRefreshToken(): string {
    return this.cookieService.get('jwt_refresh')
  }
}
