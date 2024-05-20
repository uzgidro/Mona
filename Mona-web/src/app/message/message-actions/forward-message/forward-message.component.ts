import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MessageModel, MessageRequest } from '../../../models/message';
import { UserModel } from '../../../models/user';
import { ApiService } from '../../../services/api.service';

@Component({
  selector: 'app-forward-message',
  templateUrl: './forward-message.component.html',
  styleUrl: './forward-message.component.css'
})
export class ForwardMessageComponent {

  selectedUser:UserModel
  constructor(
    public dialogRef: MatDialogRef<ForwardMessageComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      message:MessageModel,
      forwardedMessage: MessageModel,
      users:UserModel[],

    },
    private apiService:ApiService,
  ) {

  }


  onSelectUser(user:UserModel){
    console.log(user);
    this.selectedUser=user
  }

  forwardMessage(){
    let formData:FormData=new FormData()
    const messageReq:MessageRequest={
      receiverId: this.selectedUser?.id,
      createdAt: new Date(),
      forwardId:this.data.forwardedMessage.forwardId ? this.data.forwardedMessage.forwardId : this.data.forwardedMessage.id
    }
    formData.append('message',JSON.stringify(messageReq))
    this.apiService.sendMessage(formData)
    this.dialogRef.close();
  }



  cancel() {
    this.dialogRef.close();
  }

}
