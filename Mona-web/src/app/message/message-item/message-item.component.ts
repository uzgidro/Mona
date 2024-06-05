import { MatDialog } from '@angular/material/dialog';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { File, MessageModel } from '../../models/message';
import { UserModel } from '../../models/user';
import { ForwardMessageDialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-message-item',
  templateUrl: './message-item.component.html',
  styleUrl: './message-item.component.css'
})
export class MessageItemComponent {

 @Input() message?:MessageModel
 @Input() income?:MessageModel[]
 @Input() selectedChat?:UserModel
 @Input() editingMessage?: MessageModel
 @Input() repliedMessage?:MessageModel


 @Output() editMessageEvent=new EventEmitter<MessageModel>()
 @Output() deleteMessageForMyselfEvent=new EventEmitter<string>()
 @Output() deleteMessageForEveryoneEvent=new EventEmitter<string>()
 @Output() replyMessageEvent=new EventEmitter<MessageModel>()
 @Output() forwardMessageEvent=new EventEmitter<MessageModel>()
 @Output() downloadFileEvent=new EventEmitter<any>()
  forwardedMessage: any='jssk';
  users: any='ka';


  selectedMessage:MessageModel

  constructor(private dialog: MatDialog){

  }



 forwardMessageEventEmitter(){
  this.forwardMessageEvent.emit(this.message)
 }

 downloadFileEventEmitter(file:File){
  this.downloadFileEvent.emit(file)
 }

 deleteMessageForMyselfEventEmitter(){
  this.deleteMessageForMyselfEvent.emit(this.message.id)
 }
 deleteMessageForEveryoneEventEmitter(){
  this.deleteMessageForEveryoneEvent.emit(this.message.id)
 }
 editMessageEventEmitter(){
  this.editMessageEvent.emit(this.message)
 }
 replyMessageEventEmitter(){
  this.replyMessageEvent.emit(this.message)
 }


  openModal(message:MessageModel) {
    this.selectedMessage=message
   if (this.selectedMessage) {
    const dialogRef = this.dialog.open(ForwardMessageDialogComponent, {
      width: '400px',
      data: { message: this.selectedMessage }
    });
    dialogRef.afterClosed().subscribe(() => {
    });
  }
}






}
