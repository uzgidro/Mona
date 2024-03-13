import {CanActivateFn, Router} from '@angular/router';
import {inject, Injectable} from "@angular/core";

export const appGuardGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  router.navigate(['sign-up'])
  return false;
};
