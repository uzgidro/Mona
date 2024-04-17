import { Component, EventEmitter, Input, Output } from '@angular/core';
import { File, MessageModel } from '../../models/message';
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
 @Input() repliedMessage?:MessageModel


 @Output() editMessageEvent=new EventEmitter<MessageModel>()
 @Output() deleteMessageForMyselfEvent=new EventEmitter<MessageModel>()
 @Output() deleteMessageForEveryoneEvent=new EventEmitter<MessageModel>()
 @Output() replyMessageEvent=new EventEmitter<MessageModel>()
 @Output() forwardMessageEvent=new EventEmitter<MessageModel>()
 @Output() downloadFileEvent=new EventEmitter<any>()



 forwardMessageEventEmitter(){
  this.forwardMessageEvent.emit(this.message)
 }

 downloadFileEventEmitter(file:File){
  this.downloadFileEvent.emit(file)
 }

 deleteMessageForMyselfEventEmitter(){
  this.deleteMessageForMyselfEvent.emit(this.message)
 }
 deleteMessageForEveryoneEventEmitter(){
  this.deleteMessageForEveryoneEvent.emit(this.message)
 }
 editMessageEventEmitter(){
  this.editMessageEvent.emit(this.message)
 }
 replyMessageEventEmitter(){
  this.replyMessageEvent.emit(this.message)
 }





}
