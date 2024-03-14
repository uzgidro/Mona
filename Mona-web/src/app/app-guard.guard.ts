import {CanActivateChildFn, CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {JwtService} from "./services/jwt.service";

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const jwtService = inject(JwtService);
  if (jwtService.isTokenValid()) return true
  router.navigate(['auth/sign-in'])
  return false;
};

export const nonAuthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const jwtService = inject(JwtService);
  if (!jwtService.isTokenValid()) return true
  router.navigate(['message'])
  return false;
};

export const nonAuthChildGuard: CanActivateChildFn = (route, state) => {
  return nonAuthGuard(route, state)
};
