import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {  UserModel } from '../../models/user';
import { File, MessageModel, MessageRequest } from '../../models/message';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-message-actions',
  templateUrl: './message-actions.component.html',
  styleUrls: ['./message-actions.component.css']
})



export class MessageActionsComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<MessageActionsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {message:MessageModel, forwardedMessage: MessageModel,users:UserModel[], deleteMessageForMyself: (messageId:string) => void,deleteMessageForEveryone: (messageId:string) => void,},
    private apiService:ApiService
  ) {

  }

  ngOnInit() {
  }



  cancel() {
    this.dialogRef.close();
  }


  deleteForMe() {
    if (this.data.deleteMessageForMyself) {
      console.log(this.data.message);

      this.data.deleteMessageForMyself(this.data.message.id);
      this.cancel()
    }
  }


  deleteForEveryone() {
    if (this.data.deleteMessageForMyself) {
      console.log(this.data.message);

      this.data.deleteMessageForEveryone(this.data.message.id);
      this.cancel()
    }
  }




}
