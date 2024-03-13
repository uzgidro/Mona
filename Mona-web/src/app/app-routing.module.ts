import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SignUpComponent} from "./sign-up/sign-up.component";
import {BlankComponent} from "./blank/blank.component";
import {appGuardGuard} from "./app-guard.guard";
import {SignInComponent} from "./sign-in/sign-in.component";

const routes: Routes = [
  {path: '', component: BlankComponent, canActivate: [appGuardGuard]},
  {path: 'sign-up', component: SignUpComponent},
  {path: 'sign-in', component: SignInComponent},
  { path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) },
  { path: 'message', loadChildren: () => import('./message/message.module').then(m => m.MessageModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
