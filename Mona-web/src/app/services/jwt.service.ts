import {Injectable} from '@angular/core';
import {CookieService} from 'ngx-cookie-service';
import {jwtDecode} from 'jwt-decode';
import {Tokens} from "../models/tokens";

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  constructor(private cookieService: CookieService) {
  }

  saveTokens(token: Tokens) {
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

  isTokenExpired(token: string): boolean {
    const tokenPayload: { exp: number } = jwtDecode(token);
    const expiryDate: Date = new Date(tokenPayload.exp * 1000);
    return expiryDate <= new Date();
  }
  getAccessToken(): string {
    return this.cookieService.get('jwt_access')
  }

  getRefreshToken(): string {
    return this.cookieService.get('jwt_refresh')
  }
  removeTokens(){
    this.cookieService.deleteAll()
  }
  
  getIdFromJwt(): string | null {
    try {
      const access = this.getAccessToken();
      if (!access) {
        throw new Error('Access token is missing');
      }
      const decodedToken: { id?: string } = jwtDecode(access);
      const userId = decodedToken.id || null;
      if (!userId) {
        throw new Error('User ID is missing in the token');
      }
      return userId;
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

}
