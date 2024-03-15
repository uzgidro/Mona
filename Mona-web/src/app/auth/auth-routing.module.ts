import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AuthComponent} from './auth.component';
import {SignInComponent} from "./sign-in/sign-in.component";
import {SignUpComponent} from "./sign-up/sign-up.component";
import {nonAuthChildGuard, nonAuthGuard} from "../app-guard.guard";

const routes: Routes = [{
  path: '', component: AuthComponent, canActivateChild: [nonAuthChildGuard], children: [
    {
      path: '',
      redirectTo: '/auth/sign-in',
      pathMatch: 'full'
    },
    {
      path: 'sign-in',
      component: SignInComponent,
      canActivate: [nonAuthGuard]
    },
    {
      path: 'sign-up',
      component: SignUpComponent,
      canActivate: [nonAuthGuard]
    }
  ]}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
