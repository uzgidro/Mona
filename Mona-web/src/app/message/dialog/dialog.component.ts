import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {  UserModel } from '../../models/user';
import { MessageModel, MessageRequest } from '../../models/message';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-forward-message-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class ForwardMessageDialogComponent {

  selectedUser?:UserModel


  constructor(
    public dialogRef: MatDialogRef<ForwardMessageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { forwardedMessage: MessageModel,users:UserModel[]},
    private apiService:ApiService

  ) {}


  onSelectUser(user:UserModel){
    this.selectedUser=user
    console.log(this.selectedUser);
  }

  sendMessage() {
    console.log(this.selectedUser);
    console.log(this.data.forwardedMessage);

    const messageReq:MessageRequest={
      text: this.data.forwardedMessage.text,
      receiverId: this.selectedUser?.id,
      createdAt: new Date(),
      forwardId:this.data.forwardedMessage.id
    }
    console.log(messageReq);
    const formData=new FormData()
    formData.append('message',JSON.stringify(messageReq))
    this.apiService.sendMessage(formData);
  }

  cancel() {
    this.dialogRef.close();
  }
}
