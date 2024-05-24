import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {MessageRoutingModule} from './message-routing.module';
import {MessageComponent} from './message.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { MessageItemComponent } from './message-item/message-item.component';
import {MatButtonModule} from '@angular/material/button';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MessageActionsComponent } from './message-actions/message-actions.component';
import { ForwardMessageComponent } from './message-actions/forward-message/forward-message.component';
import { MatDialogContent } from '@angular/material/dialog';
import { ContactsComponent } from './contacts/contacts.component';
import { AddContactComponent } from './contacts/add-contact/add-contact.component';


@NgModule({
  declarations: [
    MessageComponent,
    MessageItemComponent,
    MessageActionsComponent,
    ForwardMessageComponent,
    ContactsComponent,
    AddContactComponent
  ],
  imports: [
    CommonModule,
    MessageRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatSidenavModule,
    MatDialogContent


  ],
})
export class MessageModule { }
