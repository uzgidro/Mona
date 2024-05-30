import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {MessageRoutingModule} from './message-routing.module';
import {MessageComponent} from './message.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { MessageItemComponent } from './message-item/message-item.component';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MessageActionsComponent } from './message-actions/message-actions.component';
import { ForwardMessageComponent } from './message-actions/forward-message/forward-message.component';
import { MatDialogContent } from '@angular/material/dialog';
import { ContactsComponent } from './contacts/contacts.component';
import { AddContactComponent } from './contacts/add-contact/add-contact.component';
import { NewGroupComponent } from './new-group/new-group.component';
import { AddMembersComponent } from './new-group/add-members/add-members.component';
import { GroupActionsComponent } from './group-actions/group-actions.component';
import {MatMenuTrigger, MatMenuModule} from '@angular/material/menu';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';


@NgModule({
  declarations: [
    MessageComponent,
    MessageItemComponent,
    MessageActionsComponent,
    ForwardMessageComponent,
    ContactsComponent,
    AddContactComponent,
    NewGroupComponent,
    AddMembersComponent,
    GroupActionsComponent,

  ],
  imports: [
    CommonModule,
    MessageRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatSidenavModule,
    MatDialogContent,
    MatButtonModule,
    MatMenuModule,
    MatIconModule


  ],
})
export class MessageModule { }
