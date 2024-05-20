import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {MessageRoutingModule} from './message-routing.module';
import {MessageComponent} from './message.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { MessageItemComponent } from './message-item/message-item.component';
import { ForwardMessageDialogComponent } from './dialog/dialog.component';
import {MatButtonModule} from '@angular/material/button';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MessageActionsComponent } from './message-actions/message-actions.component';


@NgModule({
  declarations: [
    MessageComponent,
    MessageItemComponent,
    ForwardMessageDialogComponent,
    MessageActionsComponent
  ],
  imports: [
    CommonModule,
    MessageRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatSidenavModule

  ]
})
export class MessageModule { }
