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
    @Inject(MAT_DIALOG_DATA) public data:{},

  ) {}



  cancel() {
    this.dialogRef.close();
  }
}
