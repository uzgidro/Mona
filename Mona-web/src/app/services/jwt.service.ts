import {Injectable} from '@angular/core';
import {CookieService} from "ngx-cookie-service";
import {jwtDecode} from "jwt-decode";

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

  isTokenValid() {
    const access = this.getAccessToken();
    const refresh = this.getRefreshToken();
    if (this.cookieService.check('jwt_access') && this.cookieService.check('jwt_refresh')) {
      const accessExp = new Date(jwtDecode(access).exp! * 1000);
      const refreshExp = new Date(jwtDecode(refresh).exp! * 1000);
      return accessExp > new Date() || refreshExp > new Date()
    }
    return false
  }

  getAccessToken(): string {
    return this.cookieService.get('jwt_access')
  }

  getRefreshToken(): string {
    return this.cookieService.get('jwt_refresh')
  }
}
