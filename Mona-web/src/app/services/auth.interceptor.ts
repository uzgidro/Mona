import {HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {JwtService} from "./jwt.service";
import {Injectable} from "@angular/core";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private jwtService: JwtService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = this.jwtService.getAccessToken();

    if (token) {
      const authReq = req.clone({
        setHeaders: {Authorization: `Bearer ${token}`}
      });
      return next.handle(authReq);
    }

    return next.handle(req);
  }
}
