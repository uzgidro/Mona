import { ForwardMessageComponent } from './forward-message/forward-message.component';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import {  UserModel } from '../../models/user';
import {  MessageModel} from '../../models/message';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-message-actions',
  templateUrl: './message-actions.component.html',
  styleUrls: ['./message-actions.component.css'],
}
)




export class MessageActionsComponent implements OnInit {
  selectedUser?:UserModel
  constructor(
    public dialogRef: MatDialogRef<MessageActionsComponent>,
    @Inject(MAT_DIALOG_DATA) public data:
     {
      message:MessageModel,
      forwardedMessage: MessageModel,
      users:UserModel[],
      deleteMessageForMyself: (messageId:string) => void,
      deleteMessageForEveryone: (messageId:string) => void,
      editMessage: (eventMessage: MessageModel) => void,
      currentUser:UserModel

    },
    private apiService:ApiService,
    private dialog: MatDialog
  ) {

  }

  ngOnInit() {

  }


  onSelectUser(user:UserModel){
    this.selectedUser=user
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
      this.data.deleteMessageForEveryone(this.data.message.id);
      this.cancel()
    }
  }


  copyText(){
    const messageContent = this.data.message.forwardedMessage?this.data.message.forwardedMessage.text:this.data.message.text;
    navigator.clipboard.writeText(messageContent)
      .then(() => {
        console.log('Message copied to clipboard:', messageContent);
        // Optionally, you can provide user feedback here
      })
      .catch((error) => {
        console.error('Failed to copy message to clipboard:', error);
        // Optionally, you can handle errors here
      });
      this.cancel()
  }

  editMessage(){
    this.data.editMessage(this.data.message)
    this.cancel()
  }



  forwardMessage(){
    if (this.data.message) {
      const dialogRef = this.dialog.open(ForwardMessageComponent, {
        width: '400px',
        data: {forwardedMessage: this.data.message, users: this.data.users}
      });
      dialogRef.afterClosed().subscribe(() => {
      });
    }
    this.cancel()

  }











}
