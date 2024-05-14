import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {MessageRoutingModule} from './message-routing.module';
import {MessageComponent} from './message.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { MessageItemComponent } from './message-item/message-item.component';
import { ForwardMessageDialogComponent } from './dialog/dialog.component';
import { SendMessageComponent } from './send-message/send-message.component';
import { DeleteMessageComponent } from './delete-message/delete-message.component';


@NgModule({
  declarations: [
    MessageComponent,
    MessageItemComponent,
    ForwardMessageDialogComponent,
    SendMessageComponent,
    DeleteMessageComponent
  ],
  imports: [
    CommonModule,
    MessageRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class MessageModule { }
