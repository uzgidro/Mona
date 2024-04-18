import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {  UserModel } from '../../models/user';
import { MessageModel } from '../../models/message';

@Component({
  selector: 'app-forward-message-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class ForwardMessageDialogComponent {

  selectedUser?:UserModel


  constructor(
    public dialogRef: MatDialogRef<ForwardMessageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { forwardedMessage: MessageModel,users:UserModel[]}
  ) {}


  onSelectUser(user:UserModel){
    this.selectedUser=user
    console.log(this.selectedUser);
  }

  sendMessage() {
  }

  cancel() {
    this.dialogRef.close();
  }
}
