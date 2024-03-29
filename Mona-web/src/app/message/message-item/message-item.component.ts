import { Component, Input } from '@angular/core';
import { MessageModel } from '../../models/message';
import { UserModel } from '../../models/user';

@Component({
  selector: 'app-message-item',
  templateUrl: './message-item.component.html',
  styleUrl: './message-item.component.css'
})
export class MessageItemComponent {
 @Input() message?:MessageModel
 @Input() selectedChat?:UserModel
 @Input()  editingMessage?: MessageModel
 
}
