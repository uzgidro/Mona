import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SignUpComponent} from "./sign-up/sign-up.component";
import {BlankComponent} from "./blank/blank.component";
import {appGuardGuard} from "./app-guard.guard";

const routes: Routes = [
  {path: '', component: BlankComponent, canActivate: [appGuardGuard]},
  {path: 'sign-up', component: SignUpComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
