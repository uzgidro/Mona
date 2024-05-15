import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA,MatDialog } from '@angular/material/dialog';
import {  UserModel } from '../../models/user';
import { File, MessageModel, MessageRequest } from '../../models/message';
import { ApiService } from '../../services/api.service';
import { DeleteMessageComponent } from '../delete-message/delete-message.component';
import { MessageService } from '../../services/message.service';

@Component({
  selector: 'app-forward-message-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class ForwardMessageDialogComponent {

  selectedUser?:UserModel



  constructor(
    public dialogRef: MatDialogRef<ForwardMessageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:{message: MessageModel},
    private dialog: MatDialog,
    private messageService:MessageService


  ) {}



  cancel() {
    this.dialogRef.close();
  }


  openDelete(){
    this.cancel()
    const dialogRef = this.dialog.open(DeleteMessageComponent, {
      width: '400px',
      data: { message: this.data.message}
    });
    dialogRef.afterClosed().subscribe(() => {
      // Do something after the delete dialog is closed
    });

  }



  copyMessage(message:MessageModel){
    this.messageService.copyMessage(message)
    this.dialogRef.close();
  }

  pinMessage(message:MessageModel){
    console.log(message);
    this.cancel();
  }




}
