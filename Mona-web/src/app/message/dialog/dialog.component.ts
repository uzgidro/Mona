import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {  UserModel } from '../../models/user';
import { File, MessageModel, MessageRequest } from '../../models/message';
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
  }

  sendMessages() {
    let formData:FormData=new FormData()
      const messageReq:MessageRequest={
        text: this.data.forwardedMessage.text,
        receiverId: this.selectedUser?.id,
        createdAt: new Date(),
        forwardId:this.data.forwardedMessage.id
      }
      formData.append('message',JSON.stringify(messageReq))
      this.data.forwardedMessage.files.forEach((file)=>{
        console.log(file);
      })
      this.apiService.sendMessage(formData)
      this.dialogRef.close();
  }

  cancel() {
    this.dialogRef.close();
  }
}
