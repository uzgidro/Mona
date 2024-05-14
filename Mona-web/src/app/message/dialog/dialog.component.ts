import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA,MatDialog } from '@angular/material/dialog';
import {  UserModel } from '../../models/user';
import { File, MessageModel, MessageRequest } from '../../models/message';
import { ApiService } from '../../services/api.service';
import { DeleteMessageComponent } from '../delete-message/delete-message.component';

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
    private dialog: MatDialog


  ) {}



  cancel() {
    this.dialogRef.close();
  }


  openDelete(){
    this.cancel()
    const dialogRef = this.dialog.open(DeleteMessageComponent, {
      width: '400px',
    });
    dialogRef.afterClosed().subscribe(() => {
      // Do something after the delete dialog is closed
    });

  }
}
