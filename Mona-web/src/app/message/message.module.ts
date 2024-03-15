import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {MessageRoutingModule} from './message-routing.module';
import {MessageComponent} from './message.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [
    MessageComponent
  ],
  imports: [
    CommonModule,
    MessageRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class MessageModule { }
