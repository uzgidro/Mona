import { Component, EventEmitter, Input, Output } from '@angular/core';
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


 @Output() editMessageEvent=new EventEmitter<MessageModel>()
 @Output() deleteMessageForMyselfEvent=new EventEmitter<MessageModel>()
 @Output() deleteMessageForEveryoneEvent=new EventEmitter<MessageModel>()


 deleteMessageForMyself(){
  this.deleteMessageForMyselfEvent.emit(this.message)
 }
 deleteMessageForEveryone(){
  this.deleteMessageForEveryoneEvent.emit(this.message)
 }
 editMessage(){
  this.editMessageEvent.emit(this.message)
 }

}
